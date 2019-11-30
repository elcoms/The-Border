using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using The_Border.scripts;

namespace The_Border
{
    class Program
    {
        public enum State
        {
            Menu,
            Game
        }

        private static World world = new World();
        private static Camera camera = new Camera();
        private static GraphicalInterface userInterface = new GraphicalInterface();
        
        private static bool noInput;
        private static bool play = true;
        private static bool quit;
        private static int logCount = 1;
        private static string menuTitle;
        private static State currentState = State.Menu;

        public static bool animating;
        public static Player player = new Player();
        public static Stopwatch animationTimer = new Stopwatch();
        public static Random random = new Random();

        public static List<Enemy> enemies = new List<Enemy>();
        public static List<Item> items = new List<Item>();
        public static List<Door> doors = new List<Door>();
        public static List<string> log = new List<string>();

        static void Main(string[] args)
        {
            // Set up console
            Console.WindowWidth = Constants.WINDOW_WIDTH;
            Console.WindowHeight = Constants.WINDOW_HEIGHT;

            // Remove cursor
            Console.CursorVisible = false;

            // Game loop
            RunGame();
            Console.ReadKey();
        }

        static void RunGame()
        {
            Initialize();

            while (!quit)
            {
                Update();
                Render();

                // Run game only if the game is not animating attacks
                if (!animating)
                {
                    Input();
                }
                else
                {
                    Thread.Sleep(100);

                    if (animationTimer.ElapsedMilliseconds > Constants.ATTACK_ANIM_TIME)
                    {
                        animationTimer.Reset();
                        animating = false;
                    }
                }
            }
        }

        // Load assets and prepare game
        static void Initialize()
        {
            world.Initialize();
            userInterface.Initialize();

            player.SetPosition(53, 13);

            for (int i = 1; i < doors.Count; i++)
            {
                doors[i].SetDoorColor(Constants.KEY_DOOR_COLORS[random.Next(0, 3)]);
            }

            menuTitle = File.ReadAllText(Constants.MENU_FILE);
        }
        
        // Process data not based on input
        static void Update()
        {
            player.Update();

            foreach (Enemy enemy in enemies)
            {
                enemy.Update();
            }
        }

        // Display anything on screen
        static void Render()
        {
            Console.BackgroundColor = Constants.BACKGROUND_COLOR;
            Console.ForegroundColor = Constants.FOREGROUND_COLOR;

            switch (currentState)
            {
                // ==================================================================================================
                // RENDER MENU
                // ==================================================================================================
                case State.Menu:
                    Console.Clear();
                    Console.Write(menuTitle);

                    Console.BackgroundColor = play ? ConsoleColor.White : Constants.BACKGROUND_COLOR;
                    Console.ForegroundColor = play ? ConsoleColor.Black : Constants.FOREGROUND_COLOR;
                    Console.SetCursorPosition((Constants.WINDOW_WIDTH / 2) - 5, Constants.WINDOW_HEIGHT / 2);
                    Console.Write(" Play ");

                    Console.BackgroundColor = play ? Constants.BACKGROUND_COLOR : ConsoleColor.White;
                    Console.ForegroundColor = play ? Constants.FOREGROUND_COLOR : ConsoleColor.Black;
                    Console.SetCursorPosition((Constants.WINDOW_WIDTH / 2) - 5, (Constants.WINDOW_HEIGHT / 2) + 2);
                    Console.Write(" Quit ");
                    Console.BackgroundColor = Constants.BACKGROUND_COLOR;
                    Console.ForegroundColor = Constants.FOREGROUND_COLOR;
                    break;

                // ==================================================================================================
                // RENDER GAME
                // ==================================================================================================
                case State.Game:
                    camera.Render();            // Camera must render first because it cannot be interrupted by new cursor positions
                    userInterface.Render();

                    // For debugging purposes
                    Console.SetCursorPosition(Constants.LOG_X, Constants.LOG_Y + 1);
                    foreach (string s in log)
                    {
                        Console.Write(s + " ");
                    }
                    break;
                
                default:
                    break;
            }
        }

        // Handle input
        static void Input()
        {
            ConsoleKeyInfo input = Console.ReadKey(true);
            noInput = false;

            switch (currentState)
            {
                // ==================================================================================================
                // HANDLE MENU INPUT
                // ==================================================================================================
                case State.Menu:
                    switch (input.Key)
                    {
                        case ConsoleKey.S:
                        case ConsoleKey.DownArrow:
                            play = false;
                            break;

                        case ConsoleKey.W:
                        case ConsoleKey.UpArrow:
                            play = true;
                            break;

                        case ConsoleKey.Enter:
                            if (play)
                            {
                                currentState = State.Game;
                                Console.Clear();
                            }
                            else
                                quit = true;
                            break;

                        default: noInput = true;
                            break;
                    }
                    break;

                // ==================================================================================================
                // HANDLE GAME INPUT
                // ==================================================================================================
                case State.Game:
                    switch (input.Key)
                    {
                        case ConsoleKey.Q:
                        case ConsoleKey.Escape:
                            quit = true;
                            break;

                        // MOVEMENT INPUT
                        case ConsoleKey.W:
                        case ConsoleKey.UpArrow:
                            player.OnCollision(player.X, player.Y - 1, World.GetDataFromPosition(player.X, player.Y - 1));
                            break;

                        case ConsoleKey.A:
                        case ConsoleKey.LeftArrow:
                            player.OnCollision(player.X - 1, player.Y, World.GetDataFromPosition(player.X - 1, player.Y));
                            break;

                        case ConsoleKey.S:
                        case ConsoleKey.DownArrow:
                            player.OnCollision(player.X, player.Y + 1, World.GetDataFromPosition(player.X, player.Y + 1));
                            break;

                        case ConsoleKey.D:
                        case ConsoleKey.RightArrow:
                            player.OnCollision(player.X + 1, player.Y, World.GetDataFromPosition(player.X + 1, player.Y));
                            break;


                        // INVENTORY INPUT
                        case ConsoleKey.D1:
                            player.GetInventory().DropItem(1, player.X, player.Y);
                            break;

                        case ConsoleKey.D2:
                            player.GetInventory().DropItem(2, player.X, player.Y);
                            break;

                        case ConsoleKey.D3:
                            player.GetInventory().DropItem(3, player.X, player.Y);
                            break;

                        case ConsoleKey.D4:
                            player.GetInventory().DropItem(4, player.X, player.Y);
                            break;

                        case ConsoleKey.D5:
                            player.GetInventory().DropItem(5, player.X, player.Y);
                            break;

                        default:
                            noInput = true;
                            break;
                    }
                    break;

                default:
                    break;
            }
            
        }

        // Print dialogue/narration text 
        public static void Log(string s)
        {
            // Clear log
            Console.SetCursorPosition(Constants.LOG_X, Constants.LOG_Y);
            Console.Write(new string(' ', 50));
            Console.SetCursorPosition(Constants.LOG_X, Constants.LOG_Y + 1);
            Console.Write(new string(' ', 50));

            // Write string, word wrap once if necessary
            if (s.Length > 50)
            {
                Console.SetCursorPosition(Constants.LOG_X, Constants.LOG_Y);
                Console.WriteLine(s.Substring(0, 50));
                Console.SetCursorPosition(Constants.LOG_X, Constants.LOG_Y + 1);
                Console.WriteLine(s.Substring(50));
            }
            else
            {
                Console.SetCursorPosition(Constants.LOG_X, Constants.LOG_Y);
                Console.WriteLine(s);
            }
        }

        // For debugging purposes
        public static void Log(string s, int offsetX, int offsetY)
        {
            Console.SetCursorPosition(Constants.LOG_X + offsetX, Constants.LOG_Y + offsetY);
            Console.Write(new string(' ', 200));
            Console.SetCursorPosition(Constants.LOG_X + offsetX, Constants.LOG_Y + offsetY);
            Console.WriteLine(s);
        }

        public static void LogNewLine(string s)
        {
            Console.SetCursorPosition(Constants.LOG_X, Constants.LOG_Y + logCount);
            Console.WriteLine(s);
            logCount++;
        }
    }
}

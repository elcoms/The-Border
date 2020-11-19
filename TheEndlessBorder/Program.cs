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
        
        private static bool noInput;            // true if player input any key that has no function
        private static bool play = true;        // whether play option in menu is selected
        private static bool quit;               // to check if the game has to stop running
        private static int logCount = 0;        // to count the number of moves after a dialogue is written, clear log after x moves
        private static string menuTitle;
        private static State currentState = State.Menu;

        private static World world = new World();
        private static Camera camera = new Camera();
        private static GraphicalInterface userInterface = new GraphicalInterface();
        private static Cutscene endingCutscene = new Cutscene();


        public static bool animating;           // true if animation timer is running
        public static bool dropKeyPressed;      // true if drop key is pressed;
        public static bool win;                 // true if player exits out of map and wins the game

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
            menuTitle = File.ReadAllText(Constants.MENU_FILE);

            while (!quit)
            {
                if (win)
                {
                    // Play cutscene
                    if (!endingCutscene.Play())
                    {
                        // Go back to main menu if cutscene stops playing
                        Thread.Sleep(Constants.GAMEOVER_PAUSE_TIME);
                        currentState = State.Menu;
                        play = true;
                        win = false;
                    }
                }
                // Continue updating if game not won
                else
                {
                    Update();
                    Render();

                    // Allow input only if the game is not animating attacks
                    if (!animating)
                    {
                        // If player is dead in game, read and display gameover from file, wait a few secs before going to menu
                        if (player.Dead() && currentState == State.Game)
                        {
                            Console.SetCursorPosition(0, (Constants.WINDOW_HEIGHT / 2) - 5);
                            Console.Write(File.Exists(Constants.GAMEOVER_FILE) ? File.ReadAllText(Constants.GAMEOVER_FILE) : "Game Over");

                            Thread.Sleep(Constants.GAMEOVER_PAUSE_TIME);
                            currentState = State.Menu;
                            play = true;
                            Console.Clear();
                        }
                        else
                        {
                            while (!Console.KeyAvailable)
                            {
                                Thread.Sleep(100);
                            }

                            Input();
                        }
                    }
                    else
                    {
                        Thread.Sleep(100);

                        // stop running timer if it has passed the animation time
                        if (animationTimer.ElapsedMilliseconds > Constants.ATTACK_ANIM_TIME)
                        {
                            animationTimer.Reset();
                            animating = false;
                        }
                    }
                }
            }
        }

        // Load assets and prepare game
        static void Initialize()
        {
            player = new Player();
            world = new World();
            camera = new Camera();
            enemies = new List<Enemy>();
            items = new List<Item>();
            doors = new List<Door>();
            random = new Random();
            endingCutscene = new Cutscene();

            world.Initialize();
            userInterface.Initialize();
            endingCutscene.Initialize();

            player.SetPosition(Constants.PLAYER_X, Constants.PLAYER_Y);

            for (int i = 1; i < doors.Count; i++)
            {
                if (doors[i].GetDoorColor() != ConsoleColor.Gray)
                    doors[i].SetDoorColor(Constants.KEY_DOOR_COLORS[random.Next(0, 3)]);
            }

            logCount = 0;
            Log("The Man is confused. He feels trapped.");
        }
        
        // Process data not based on input
        static void Update()
        {
            if (currentState == State.Game)
            {
                player.Update();

                foreach (Enemy enemy in enemies)
                {
                    enemy.Update();
                }
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

            // Clear input buffer
            while (Console.KeyAvailable)
                Console.ReadKey(true);

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
                                Initialize();
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

                    if (logCount > 5)
                    {
                        Log(new string(' ', 100));
                    }

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
                            if (dropKeyPressed)
                                player.GetInventory().DropItem(1, player.X, player.Y);
                            else
                                player.GetInventory().UseItem(1);
                            break;

                        case ConsoleKey.D2:
                            if (dropKeyPressed)
                                player.GetInventory().DropItem(2, player.X, player.Y);
                            else
                                player.GetInventory().UseItem(2);
                            break;

                        case ConsoleKey.D3:
                            if (dropKeyPressed)
                                player.GetInventory().DropItem(3, player.X, player.Y);
                            else
                                player.GetInventory().UseItem(3);
                            break;

                        case ConsoleKey.D4:
                            if (dropKeyPressed)
                                player.GetInventory().DropItem(4, player.X, player.Y);
                            else
                                player.GetInventory().UseItem(4);
                            break;

                        case ConsoleKey.D5:
                            if (dropKeyPressed)
                                player.GetInventory().DropItem(5, player.X, player.Y);
                            else
                                player.GetInventory().UseItem(5);
                            break;

                        case ConsoleKey.Spacebar:
                            dropKeyPressed = !dropKeyPressed;

                            Log(dropKeyPressed ? "The Man thinks about what he wants to leave behind..."
                                : "The Man stops thinking about leaving the items and starts thinking about leaving The Border.");
                            break;

                        default:
                            noInput = true;
                            break;
                    }

                    if (!noInput)
                        logCount++;
                    break;

                default:
                    break;
            }
            
        }

        // Print dialogue/narration text 
        public static void Log(string s)
        {
            // start counting the number of moves
            logCount = 0;

            // Clear log
            Console.SetCursorPosition(Constants.LOG_X, Constants.LOG_Y);
            Console.Write(new string(' ', 85));
            Console.SetCursorPosition(Constants.LOG_X, Constants.LOG_Y + 1);
            Console.Write(new string(' ', 85));

            // Write string, word wrap once if necessary
            if (s.Length > 85)
            {
                Console.SetCursorPosition(Constants.LOG_X, Constants.LOG_Y);
                Console.WriteLine(s.Substring(0, 85));
                Console.SetCursorPosition(Constants.LOG_X, Constants.LOG_Y + 1);
                Console.WriteLine(s.Substring(85));
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
    }
}

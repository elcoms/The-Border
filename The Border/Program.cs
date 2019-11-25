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
        private static World world = new World();
        private static Player player = new Player();
        
        
        private static bool noInput;
        private static bool quit;

        public static Stopwatch animationTimer = new Stopwatch();
        public static List<Enemy> enemies = new List<Enemy>();
        public static List<Item> items = new List<Item>();
        public static List<Door> doors = new List<Door>();

        static void Main(string[] args)
        {
            // Set up console
            Console.WindowWidth = Console.LargestWindowWidth / 2;
            Console.WindowHeight = Console.LargestWindowHeight / 2;

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

                // run input only if the game is not animating attacks
                if (!animationTimer.IsRunning)
                    Input();
                else
                    Thread.Sleep(100);
            }
        }

        // Load assets and prepare game
        static void Initialize()
        {
            world.Initialize();
        }

        // Display anything on screen
        static void Render()
        {
            Console.BackgroundColor = Constants.BACKGROUND_COLOR;
            Console.ForegroundColor = Constants.FOREGROUND_COLOR;

            world.Render();
            player.Render();
        }

        // Handle input
        static void Input()
        {
            ConsoleKeyInfo input = Console.ReadKey(true);
            noInput = false;

            switch (input.Key)
            {
                case ConsoleKey.Q:
                case ConsoleKey.Escape:
                    quit = true;
                    break;

                // MOVEMENT INPUT
                case ConsoleKey.W:
                case ConsoleKey.UpArrow:
                    world.CheckCollision(player.X, player.Y - 1, player);
                    break;

                case ConsoleKey.A:
                case ConsoleKey.LeftArrow:
                    world.CheckCollision(player.X - 1, player.Y, player);
                    break;

                case ConsoleKey.S:
                case ConsoleKey.DownArrow:
                    world.CheckCollision(player.X, player.Y + 1, player);
                    break;

                case ConsoleKey.D:
                case ConsoleKey.RightArrow:
                    world.CheckCollision(player.X + 1, player.Y, player);
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

                default: noInput = true;
                    break;
            }
        }

        // Process data not based on input
        static void Update()
        {
            foreach (Enemy enemy in Program.enemies)
            {
                enemy.Update();
            }
        }

        // Print dialogue/narration text 
        public static void Log(string s)
        {
            Console.SetCursorPosition(5, 18);
            Console.Write(new string(' ', 200));
            Console.SetCursorPosition(5, 18);
            Console.WriteLine(s);
        }

        // For debugging purposes
        public static void Log(string s, int offsetX, int offsetY)
        {
            Console.SetCursorPosition(5 + offsetX, 18 + offsetY);
            Console.Write(new string(' ', 200));
            Console.SetCursorPosition(5 + offsetX, 18 + offsetY);
            Console.WriteLine(s);
        }
    }
}

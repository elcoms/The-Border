using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using The_Border.scripts;

namespace The_Border
{
    class Program
    {
        private static World world = new World();
        private static Player player = new Player();
        private static List<Enemy> enemies = new List<Enemy>();
        private static Random random = new Random();

        private static string worldString;
        private static bool noInput;
        private static bool quit;
        static void Main(string[] args)
        {
            // Set up console
            // Console.WindowWidth = Console.LargestWindowWidth;
            // Console.WindowHeight = Console.LargestWindowHeight;

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
                Console.Clear();
                
                Update();
                Render();
                Input();
            }
        }

        // Load assets and prepare game
        static void Initialize()
        {
            worldString = File.ReadAllText(Constants.DUNGEON_FILE);
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

                default: noInput = true;
                    break;
            }
        }

        // Process data not based on input
        static void Update()
        {

        }

        // For debugging purposes
        static void Log(string s)
        {
            Console.SetCursorPosition(18, 5);
            Console.Write(s);
        }
    }
}

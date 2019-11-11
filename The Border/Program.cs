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
        private static Player player = new Player();
        private static World world = new World();
        private static string worldString;
        private static bool quit;
        static void Main(string[] args)
        {
            // Set up console
            // Console.WindowWidth = Console.LargestWindowWidth;
            // Console.WindowHeight = Console.LargestWindowHeight;

            Enemy[] enemies = new Enemy[20];
            for (int i = 0; i < enemies.Length; ++i)
            {
                enemies[i] = new Enemy();
            }

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

                Render();
                Input();
                Update();
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
            Console.Write(worldString);
            // world.Render();

            player.Render();
        }

        // Handle input
        static void Input()
        {
            ConsoleKeyInfo input = Console.ReadKey(true);

            switch (input.Key)
            {
                case ConsoleKey.Q:
                case ConsoleKey.Escape:
                    quit = true;
                    break;

                case ConsoleKey.W:
                case ConsoleKey.UpArrow:
                    if (!world.CollidedWithWall(player.X, player.Y - 1))
                        player.Move(0, -1);
                    break;

                case ConsoleKey.A:
                case ConsoleKey.LeftArrow:
                    if (!world.CollidedWithWall(player.X - 1, player.Y))
                        player.Move(-1, 0);
                    break;

                case ConsoleKey.S:
                case ConsoleKey.DownArrow:
                    if (!world.CollidedWithWall(player.X, player.Y + 1))
                        player.Move(0, 1);
                    break;

                case ConsoleKey.D:
                case ConsoleKey.RightArrow:
                    if (!world.CollidedWithWall(player.X + 1, player.Y))
                        player.Move(1, 0);
                    break;

                default:
                    break;
            }
        }

        // Process other data based on input
        static void Update()
        {

        }
    }
}

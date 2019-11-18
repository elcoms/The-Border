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

            for (int i = 0; i < 2; i++)
            {
                Enemy enemy = new Enemy(random.Next(1, 3), random.Next(1, 3), 10, 5);
                enemies.Add(enemy);
            }
        }

        // Display anything on screen
        static void Render()
        {
            world.Render();
            player.Render();

            foreach (Enemy enemy in enemies)
            {
                enemy.Render();
            }
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
                    if (!world.CollidedWithWall(player.X, player.Y - 1) && !CollidedWithEnemy(player.X, player.Y - 1))
                        player.Move(0, -1);
                    break;

                case ConsoleKey.A:
                case ConsoleKey.LeftArrow:
                    if (!world.CollidedWithWall(player.X - 1, player.Y) && !CollidedWithEnemy(player.X - 1, player.Y))
                        player.Move(-1, 0);
                    break;

                case ConsoleKey.S:
                case ConsoleKey.DownArrow:
                    if (!world.CollidedWithWall(player.X, player.Y + 1) && !CollidedWithEnemy(player.X, player.Y + 1))
                        player.Move(0, 1);
                    break;

                case ConsoleKey.D:
                case ConsoleKey.RightArrow:
                    if (!world.CollidedWithWall(player.X + 1, player.Y) && !CollidedWithEnemy(player.X + 1, player.Y))
                        player.Move(1, 0);
                    break;

                default:
                    break;
            }
        }

        // Process data based on input
        static void Update()
        {

        }

        static bool CollidedWithEnemy(int x, int y)
        {
            foreach (Enemy enemy in enemies)
            {
                if (enemy.X == x && enemy.Y == y)
                    return true;
            }

            return false;
        }
    }
}

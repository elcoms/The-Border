using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static The_Border.Constants;
using The_Border.scripts;

namespace The_Border
{
    class Program
    {
        private static Player player = new Player();
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
            while (true)
            {
                Console.Clear();

                Render();
                Input();
                Update();
            }
        }

        static void Render()
        {
            string text = File.ReadAllText(DUNGEON_FILE);
            Console.WriteLine(text);

            player.Render();
        }

        static void Input()
        {
            Console.ReadKey(true);
        }

        static void Update()
        {

        }
    }
}

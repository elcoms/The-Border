using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace The_Border.scripts
{
    class World
    {
        char[,] worldData;

        public World() {
            worldData = new char[Constants.WORLD_WIDTH, Constants.WORLD_HEIGHT];
        }

        public void Initialize()
        {
            if(File.Exists(Constants.DUNGEON_FILE))
            {
                StreamReader reader = new StreamReader(Constants.DUNGEON_FILE);
                string line = reader.ReadLine();

                int x = 0, y = 0;
                // loop as long as there's another line
                while (line != null)
                {
                    // Console.ReadKey(true);
                    // Console.WriteLine(world[i] + ": " + x + ", " + y);
                    foreach (char c in line)
                    {
                        // assign the character to a 2D array according to x and y
                        worldData[x, y] = c;

                        x++;
                    }

                    x = 0;
                    y++;
                    line = reader.ReadLine();
                }
            }
            else
            {
                Console.WriteLine("World file not found.");
            }
        }

        // For testing purposes
        public void Render()
        {
            for (int y = 0; y < worldData.GetLength(0); y++)
            {
                for (int x = 0; x < worldData.GetLength(1); x++)
                {
                    Console.Write(worldData[x, y]);
                }
                Console.WriteLine();
            }
        }

        public char GetCollidedObject(int x, int y)
        {
            // Check if going out of bounds
            if (x < 0 || y < 0)
                return Constants.OUT_OF_BOUNDS;
            else
                return worldData[x, y];
        }

        public void UpdateObjectPosition(int x, int y, char c)
        {
            worldData[x, y] = c;
        }
    }
}

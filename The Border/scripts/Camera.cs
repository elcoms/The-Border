using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace The_Border.scripts
{
    class Camera
    {
        static char[,] visibleMap;
        string visible;

        public Camera()
        {
            visibleMap = new char[Constants.WORLD_WIDTH, Constants.WORLD_HEIGHT];
        }

        // Load the data for the visible map
        public void Initialize()
        {
            // Check if file exists
            if (File.Exists(Constants.DUNGEON_FILE))
            {
                StreamReader reader = new StreamReader(Constants.DUNGEON_FILE);
                string line = reader.ReadLine();

                int x = 0, y = 0;
                // loop as long as there's another line
                while (line != null)
                {
                    foreach (char c in line)
                    {
                        visibleMap[x, y] = c;

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

        public void Update()
        {
            // update visible string
            // =====================================
            visible = string.Empty;

            // starting positions to get from world: top left of player
            int startX = Program.player.X - (Constants.CAM_WIDTH / 2);
            int startY = Program.player.Y - (Constants.CAM_HEIGHT / 2);
            
            // check if out of bounds
            if (startX < 0)
                startX = 0;

            if (startX > Constants.WORLD_WIDTH)
                startX = Constants.WORLD_WIDTH - Constants.CAM_WIDTH;

            if (startY < 0)
                startY = 0;

            if (startX > Constants.WORLD_HEIGHT)
                startY = Constants.WORLD_HEIGHT - Constants.CAM_HEIGHT;
            int count = 0;
            // get data from world to render starting from the start positions to the camera size
            while (startY < Constants.CAM_HEIGHT)
            {
                while (startX < Constants.CAM_WIDTH)
                {
                    visible += visibleMap[startX, startY];

                    Console.WriteLine(count++ + ": " + visibleMap[startX, startY]);
                    startX++;
                    count++;
                }
                visible += Environment.NewLine;
                startY++;
            }
        }

        public void Render()
        {
            Console.SetCursorPosition(5, 5);
            Console.Write(visible);
        }

        public static void UpdateVisibleMap(int x, int y, char data)
        {
            if (visibleMap[x, y] == Constants.SPACE)
                visibleMap[x, y] = data;
        }
    }
}

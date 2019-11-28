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
        string visible, clear;

        public Camera()
        {
            visibleMap = new char[Constants.WORLD_WIDTH, Constants.WORLD_HEIGHT];

            for (int i = 0; i < Constants.CAM_HEIGHT; i++)
            {
                clear += new string(' ', Constants.CAM_WIDTH);
                clear += Environment.NewLine;
            }
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
            
            // get data from world to render starting from the start positions to the camera size
            for (int y = 0; y < Constants.CAM_HEIGHT; y++)
            {
                for (int x = 0; x < Constants.CAM_WIDTH; x++)
                {
                    visible += visibleMap[startX + x, startY + y];
                }
                visible += Environment.NewLine;
                visible += new string(' ', Constants.CAM_START_X);
            }
        }

        public void Render()
        {
            Console.SetCursorPosition(Constants.CAM_START_X, Constants.CAM_START_Y);
            Console.Write(visible);
        }

        public static void UpdateVisibleMap(int x, int y, char data)
        {
            if (visibleMap[x, y] == Constants.SPACE)
                visibleMap[x, y] = data;
        }
    }
}

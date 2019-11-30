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
        static Object[,] visibleMap;

        public Camera()
        {
            visibleMap = new Object[Constants.WORLD_WIDTH, Constants.WORLD_HEIGHT];
        }

        public void Render()
        {
            // starting positions to get from world: top left of player
            int startX = Program.player.X - (Constants.CAM_WIDTH / 2);
            int startY = Program.player.Y - (Constants.CAM_HEIGHT / 2);

            // check if out of bounds
            if (startX < 0)
                startX = 0;

            if (startX > Constants.WORLD_WIDTH - Constants.CAM_WIDTH)
                startX = Constants.WORLD_WIDTH - Constants.CAM_WIDTH;

            if (startY < 0)
                startY = 0;

            if (startX > Constants.WORLD_HEIGHT - Constants.CAM_HEIGHT)
                startY = Constants.WORLD_HEIGHT - Constants.CAM_HEIGHT;

            // get data from world to render starting from the start positions to the camera size
            Console.SetCursorPosition(Constants.CAM_START_X, Constants.CAM_START_Y);
            for (int y = 0; y < Constants.CAM_HEIGHT; y++)
            {
                for (int x = 0; x < Constants.CAM_WIDTH; x++)
                {
                    visibleMap[startX + x, startY + y]?.Render();
                }
                Console.WriteLine();
                Console.Write(new string(' ', Constants.CAM_START_X));
            }
        }

        // set an element according to the data position to the data
        public static void UpdateVisibleMap(Object data)
        {
           visibleMap[data.X, data.Y] = data;
        }

        // set an element in the array to the data
        public static void UpdateVisibleMap(int x, int y, Object data)
        {
            visibleMap[x, y] = data;
        }
    }
}

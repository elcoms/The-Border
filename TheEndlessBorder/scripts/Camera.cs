using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TheEndlessBorder.scripts
{
    class Camera
    {
        World world;

        public Camera(World world)
        {
            this.world = world;
        }

        public void Render()
        {
            // starting positions to get from world: top left of player
            int startX = Program.player.X - (Constants.CAM_WIDTH / 2);
            int startY = Program.player.Y - (Constants.CAM_HEIGHT / 2);

            // get data from world to render starting from the start positions to the camera size
            Console.SetCursorPosition(Constants.CAM_START_X, Constants.CAM_START_Y);
            Object[,] worldObjects = World.GetWorldObjects();

            /*
         for (int y = 0; y < worldObjects.GetLength(1); y++)
         {
             Console.SetCursorPosition(0, y);
             for (int x = 0; x < worldObjects.GetLength(0); x++)
             {
                 // Check if the world exists in this position
                 if (x < 0 || y < 0 || x >= worldObjects.GetLength(0) || y >= worldObjects.GetLength(1))
                 {
                     // Render an empty space if not
                     Console.Write(Constants.SPACE);
                 }
                 else // Render the object if it exists
                 {
                     if (worldObjects[x, y] != null)
                         worldObjects[x, y].Render();
                     else
                         Console.Write(Constants.SPACE);
                 }
             }
         }
            */
            // From starting world positions up to a length set by constants
            for (int y = startY; y < startY + Constants.CAM_HEIGHT; y++)
            {
                for (int x = startX; x < startX + Constants.CAM_WIDTH; x++)
                {
                    // Check if the world exists in this position
                    if (x < 0 || y < 0 || x >= worldObjects.GetLength(0) || y >= worldObjects.GetLength(1))
                    {
                        // Render an empty space if not
                        Console.Write(Constants.SPACE);
                    }
                    else // Render the object if it exists
                    {
                        if (worldObjects[x, y] != null)
                            worldObjects[x, y].Render();
                        else
                            Console.Write(Constants.SPACE);
                    }
                }

                // Padding
                Console.SetCursorPosition(Constants.CAM_START_X, y - startY + 3);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Border.scripts
{
    class Door
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        ConsoleColor color = ConsoleColor.DarkRed;

        
        string name = "The Door";
        bool unlocked = false;

        public Door(int xPos, int yPos)
        {
            X = xPos;
            Y = yPos;
        }

        public Door(int xPos, int yPos, string givenName, ConsoleColor givenColor)
        {
            X = xPos;
            Y = yPos;
            name = givenName;
            color = givenColor;
        }

        public void Render()
        {
            if (unlocked)
            {
                Console.SetCursorPosition(X, Y);
                Console.Write(Constants.SPACE);
            }
            else
            {
                Console.ForegroundColor = color;
                Console.SetCursorPosition(X, Y);
                Console.Write(Constants.DOOR);
            }

            Console.ForegroundColor = Constants.FOREGROUND_COLOR;
        }

        public void OnCollision(Player player)
        {
            // Check for key
            foreach (Item item in player.GetInventory().GetItems())
            {
                if (item as Key != null)
                {
                    if (item.getColor() == color)
                    {
                        World.UpdateWorldData(X, Y, Constants.SPACE);
                        unlocked = true;
                    }   
                }
            }

            // print dialogue
            if (unlocked)
            {
                Program.Log("The Door is happy to be reunited with The Key.");
            }
            else
            {
                Program.Log("The Man fidgets and pulls but The Door did not seem to care.");
            }
        }
    }
}

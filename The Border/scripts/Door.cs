using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Border.scripts
{
    class Door : Object
    {
        ConsoleColor color = ConsoleColor.DarkRed;

        
        string name = "The Door";
        bool unlocked = false;
        bool horizontal = false;

        public Door(int xPos, int yPos, bool isHorizontal)
        {
            X = xPos;
            Y = yPos;
            horizontal = isHorizontal;
        }

        public Door(int xPos, int yPos, string givenName, ConsoleColor givenColor, bool isHorizontal)
        {
            X = xPos;
            Y = yPos;
            name = givenName;
            color = givenColor;
            horizontal = isHorizontal;
        }

        public void SetDoorColor(ConsoleColor newColor) { color = newColor; }

        public override void Render()
        {
            if (unlocked)
            {
                Console.Write(Constants.SPACE);
            }
            else
            {
                Console.ForegroundColor = color;
                Console.Write(horizontal ? Constants.DOOR_HORIZONTAL : Constants.DOOR_VERTICAL);
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

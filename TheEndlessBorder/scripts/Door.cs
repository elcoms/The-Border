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

        
        string name = "The DarkRed Door";
        bool unlocked = false;
        bool horizontal = false;

        public Door(int xPos, int yPos, bool isHorizontal)
        {
            X = xPos;
            Y = yPos;
            horizontal = isHorizontal;
            sprite = isHorizontal ? Constants.DOOR_HORIZONTAL : Constants.DOOR_VERTICAL;
        }

        public Door(int xPos, int yPos, string givenName, ConsoleColor givenColor, bool isHorizontal)
        {
            X = xPos;
            Y = yPos;
            name = givenName;
            color = givenColor;
            horizontal = isHorizontal;
            sprite = isHorizontal ? Constants.DOOR_HORIZONTAL : Constants.DOOR_VERTICAL;
        }

        public override void Render()
        {
            if (unlocked)
            {
                Console.Write(Constants.SPACE);
            }
            else
            {
                Console.ForegroundColor = color;
                Console.Write(sprite);
            }

            Console.ForegroundColor = Constants.FOREGROUND_COLOR;
        }

        public void OnCollision(Player player)
        {
            // unlock if it's fence
            if (color == ConsoleColor.Gray)
            {
                World.UpdateWorldData(X, Y, Constants.SPACE);
                unlocked = true;

                Program.Log("The Man breaks " + name + " and escapes The Border.");
            }
            else
            {
                Key rightKey = null;

                // Check for key
                foreach (Item item in player.GetInventory().GetItems())
                {
                    if (item as Key != null)
                    {
                        if (item.getColor() == color)
                        {
                            World.UpdateWorldData(X, Y, Constants.SPACE);
                            rightKey = item as Key;
                            player.Level++;
                            player.GetInventory().RemoveItem(item);
                            unlocked = true;
                        }
                    }
                }

                // print dialogue
                if (unlocked)
                {
                    Program.Log(name + " is happy to be reunited with " + (rightKey != null ? rightKey.Name : " The Key"));
                }
                else
                {
                    Program.Log("The Man fidgets and pulls but " + name + " did not seem to care.");
                }
            }
        }

        // Getter/Setters
        public ConsoleColor GetDoorColor() { return color; }
        public void SetDoorColor(ConsoleColor newColor)
        {
            color = newColor;

            // rename door according to color
            name = "The " + color.ToString() + " Door";
        }
    }
}

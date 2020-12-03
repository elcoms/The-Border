using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheEndlessBorder.scripts
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

        public Door(int xPos, int yPos, bool isHorizontal, ConsoleColor givenColor)
        {
            X = xPos;
            Y = yPos;
            horizontal = isHorizontal;
            sprite = isHorizontal ? Constants.DOOR_HORIZONTAL : Constants.DOOR_VERTICAL;
            SetDoorColor(givenColor);
        }

        public override void Render()
        {
            Console.ForegroundColor = color;
            Console.Write(sprite);
            Console.ForegroundColor = Constants.FOREGROUND_COLOR;
        }

        public void OnCollision(Player player)
        {
            // unlock if it's fence
            if (color == ConsoleColor.Gray)
            {
                World.UpdateWorldObjects(new Object(X, Y, Constants.FLOOR));
                unlocked = true;

                Program.Log("The Man breaks " + name + " and escapes The Border.");
            }
            else
            {
                Key rightKey = null;
                World.UpdateWorldObjects(new Object(X, Y, Constants.FLOOR));
                World.CreateNewRoom(new Vector2(X, Y));
                unlocked = true;

                // Check for key
                foreach (Item item in player.GetInventory().GetItems())
                {
                    if (item as Key != null)
                    {
                        if (item.getColor() == color)
                        {
                            rightKey = item as Key;
                            player.Level++;
                            player.GetInventory().RemoveItem(item);
                            World.CreateNewRoom(new Vector2(X, Y));
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Border.scripts
{
    class Door
    {
        ConsoleColor color = ConsoleColor.DarkRed;

        int x = 1, y = 1;
        string name = "The Door";
        bool unlocked = false;

        public Door (int xPos, int yPos, string givenName, ConsoleColor givenColor)
        {
            x = xPos;
            y = yPos;
            name = givenName;
            color = givenColor;
        }

        public void Render()
        {
            Console.ForegroundColor = color;
            Console.SetCursorPosition(x, y);
            Console.Write(Constants.DOOR);
        }

        public OnCollision(Player player)
        {
            // Check for key
            // if key, Log: && unlocked = true;
            // else Log:
        }
    }
}

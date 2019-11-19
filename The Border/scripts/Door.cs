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
            Console.ForegroundColor = color;
            Console.SetCursorPosition(X, Y);
            Console.Write(Constants.DOOR);

            Console.ForegroundColor = Constants.FOREGROUND_COLOR;
        }

        public void OnCollision(Player player)
        {
            // Check for key
            // if key, Log: && unlocked = true;
            // else Log:
        }
    }
}

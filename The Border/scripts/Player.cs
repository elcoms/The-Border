using System;

using static The_Border.Constants;

namespace The_Border.scripts
{
    class Player
    {
        int X { get; set; }
        int Y { get; set; }

        public Player()
        {
            X = 5;
            Y = 5;
        }

        public void Render()
        {
            Console.SetCursorPosition(X, Y);
            Console.Write(PLAYER);
        }

        public void Input()
        {

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Border.scripts
{
    class Item : Object
    {
        public string Name { get; protected set; }
        private bool isVisible = true;

        protected new char sprite = Constants.LOOTBAG;
        protected ConsoleColor color = Constants.FOREGROUND_COLOR;

        public Item()
        {
            Name = "The Item";
        }

        public override void Render()
        {
            Console.SetCursorPosition(X, Y);

            if (isVisible)
            {
                Console.ForegroundColor = color;
                Console.Write(sprite);
                Console.ForegroundColor = Constants.FOREGROUND_COLOR;
            }
            else
            {
                Console.Write(Constants.SPACE);
            }
        }

        public void SetVisible(bool visible)
        {
            isVisible = visible;
        }

        public char getSprite() { return sprite; }
        public ConsoleColor getColor() { return color; }
    }
}

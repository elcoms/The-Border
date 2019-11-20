using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Border.scripts
{
    class Item : Object
    {
        protected string name = "The Item";
        protected new char sprite = Constants.LOOTBAG;
        protected ConsoleColor color = Constants.FOREGROUND_COLOR;

        public override void Render()
        {
            Console.SetCursorPosition(X, Y);
            Console.ForegroundColor = color;
            Console.Write(sprite);
        }
        public virtual void OnCollision(Player player)
        {

        }
    }
}

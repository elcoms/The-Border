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
        public Character Holder { get; set; }
        
        private bool isVisible = true;

        protected ConsoleColor color = Constants.FOREGROUND_COLOR;

        public Item()
        {
            Name = "The Item";
            sprite = 'I';
        }

        public override void Render()
        {
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

        public virtual void Use(Player player) { }

        public void SetVisible(bool visible)
        {
            isVisible = visible;
        }

        public ConsoleColor getColor() { return color; }
    }
}

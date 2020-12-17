using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheEndlessBorder.scripts
{
    class Item : Object
    {
        public string Name { get; protected set; }
        public Character Holder { get; set; }
        
        protected bool isVisible = true;

        protected ConsoleColor color = Constants.FOREGROUND_COLOR;
        protected ConsoleColor unlitColor = ConsoleColor.DarkGray;

        public Item()
        {
            Name = "The Item";
            sprite = 'I';
            isLit = true;
        }

        public override void Render()
        {
            if (isVisible)
            {
                Console.ForegroundColor = isLit ? color : unlitColor;
                Console.Write(sprite);
                Console.ForegroundColor = Constants.FOREGROUND_COLOR;
            }
        }

        public virtual void Use(Player player) { }

        public virtual void OnPickUp(Player player) 
        {
            player.GetInventory().AddItem(this);
        }

        public virtual void OnDrop() 
        {
            objectInBackground = World.GetObjectFromPosition(X, Y);
        }

        public void SetVisible(bool visible)
        {
            isVisible = visible;
        }

        public ConsoleColor getColor() { return color; }
    }
}

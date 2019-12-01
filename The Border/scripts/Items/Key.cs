using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Border.scripts
{
    class Key : Item
    {
        public Key(int x, int y, ConsoleColor keyColor)
        {
            X = x;
            Y = y;
            sprite = Constants.KEY;
            color = keyColor;

            // name key according to color
            Name = "The " + color.ToString() + " Key";
        }

        public override void Use(Player player)
        {
            Program.Log("The Man stares at " + Name + ". It looks lonely.");
        }
    }
}

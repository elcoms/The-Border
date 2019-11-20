using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Border.scripts
{
    class Key : Item
    {
        public Key(int x, int y, ConsoleColor keyColor, string name)
        {
            X = x;
            Y = y;
            sprite = Constants.KEY;
            color = keyColor;
            Name = name;
        }
    }
}

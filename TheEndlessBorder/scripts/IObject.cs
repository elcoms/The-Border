using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Border.scripts
{
    interface IObject
    {
        int X { get; set; }
        int Y { get; set; }

        void Render();

        void Move(int x, int y);
    }
}

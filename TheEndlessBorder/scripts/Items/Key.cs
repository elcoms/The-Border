using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheEndlessBorder.scripts
{
    class Key : Item
    {
        public Key(int x, int y, int keyColor)
        {
            X = x;
            Y = y;
            sprite = Constants.KEY;
            color = Constants.KEY_DOOR_COLORS[keyColor];
            unlitColor = Constants.KEY_DOOR_COLORS_DARK[keyColor];
            isLit = true;

            // name key according to color
            Name = "The " + color.ToString() + " Key";
        }

        public override void Use(Player player)
        {
            Program.Log("The Man stares at " + Name + ". It looks lonely.");
        }
    }
}

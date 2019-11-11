using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Border.scripts
{
    class Enemy
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        private int health, attack;

        public Enemy()
        {
            X = 5;
            Y = 5;
            health = 100;
            attack = 5;
        }
        public void Render()
        {
            Console.SetCursorPosition(X, Y);
            Console.Write(Constants.PLAYER);
        }

        public void Damage()
        {

        }

        public void Heal(int amt)
        {
            health += amt;
        }

        public void Move(int xAmt, int yAmt)
        {
            X += xAmt;
            Y += yAmt;
        }
    }
}

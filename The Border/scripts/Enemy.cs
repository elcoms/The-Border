using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Border.scripts
{
    class Enemy : Character
    {
        public Enemy()
        {
            X = 5;
            Y = 5;
            health = 100;
            attack = 5;
        }

        public Enemy(int xPos, int yPos, int hp, int attackPower)
        {
            X = xPos;
            Y = yPos;
            health = hp;
            attack = attackPower;
        }

        public override void Render()
        {
            Console.SetCursorPosition(X, Y);

            // render enemy if alive
            if (health > 0)
                Console.Write(Constants.ENEMY);
            // render enemy dead body if dead
            else if (!dead)
            {
                Console.Write(Constants.ENEMY_DEAD);
                dead = true;
            }   
            // don't render anything after enemy dead
        }
    }
}

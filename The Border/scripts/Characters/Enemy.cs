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
            X = 1;
            Y = 1;
            health = 100;
            damage = 5;
        }

        public Enemy(int xPos, int yPos, int hp, int attackPower)
        {
            X = xPos;
            Y = yPos;
            health = hp;
            damage = attackPower;
        }

        public override void Render()
        {
            Console.SetCursorPosition(X, Y);
            
            // render enemy dead body if dead
            if (!dead)
            {
                // render enemy if alive
                if (health > 0)
                    Console.Write(Constants.ENEMY);
                // render dead body if not, update world data to be walkable
                else
                {
                    Console.Write(Constants.ENEMY_DEAD);
                    World.UpdateWorldData(X, Y, Constants.SPACE);
                    dead = true;
                }
            }
            // don't render anything after enemy dead
            else
            {
                Console.Write(Constants.SPACE);
            }
        }
    }
}

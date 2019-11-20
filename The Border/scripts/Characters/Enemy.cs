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
            sprite = Constants.ENEMY;
        }

        public Enemy(int xPos, int yPos, int hp, int attackPower, char character)
        {
            X = xPos;
            Y = yPos;
            health = hp;
            damage = attackPower;
            sprite = character;
        }
    }
}

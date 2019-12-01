using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Border.scripts
{
    class PatrolEnemy : Enemy
    {
        private bool actionUsed;    // true if the enemy already did an action (moving/attacking)

        public PatrolEnemy()
        {
            X = 1;
            Y = 1;
            health = 50;
            damage = 5;
            sprite = Constants.ENEMY_PATROL;
        }
        public PatrolEnemy(int xPos, int yPos, int hp, int attackPower, char character, Item item)
        {
            X = xPos;
            Y = yPos;
            health = hp;
            damage = attackPower;
            sprite = character;
            drop = item;
            drop.SetVisible(false);
            drop.Holder = this;
        }

        public override void Update()
        {
            actionUsed = false;     // Reset action

            base.Update();
        }

        public override void OnCollision(int x, int y, char collision)
        {
            switch (collision)
            {
                case Constants.PLAYER:
                    if (!attack && !Program.animating && !actionUsed)
                    {
                        Program.player.Damaged(damage, this);
                        attack = true;
                        actionUsed = true;
                    }
                    break;

                case Constants.SPACE:
                    if (!actionUsed)
                    {
                        if (Program.random.Next(0, 10) < 3)
                        {
                            SetPosition(x, y);
                            actionUsed = true;
                        }
                    }
                    break;

                default:
                    break;
            }
        }
    }
}

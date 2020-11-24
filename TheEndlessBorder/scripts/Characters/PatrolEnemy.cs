using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheEndlessBorder.scripts
{
    class PatrolEnemy : Enemy
    {
        private bool actionUsed;    // true if the enemy already did an action (moving/attacking)
        private Random random = new Random();

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

        public override void OnCollision(Object collidedObject)
        {
            switch (collidedObject.GetSprite())
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
                        // 30% chance of moving
                        if (random.Next(0, 10) < 3)
                        {
                            SetPosition(collidedObject.X, collidedObject.Y);
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

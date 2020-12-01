using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheEndlessBorder.scripts
{
    class Enemy : Character
    {
        protected Item drop;
        protected bool attack;

        public Enemy()
        {
            X = 1;
            Y = 1;
            health = 100;
            damage = 5;
            sprite = Constants.ENEMY;
        }

        public Enemy(int xPos, int yPos, int hp, int attackPower, char character, Item item)
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

        public override void Render()
        {
            base.Render();
        }

        public void DropItem()
        {
            drop.SetPosition(X, Y);
            drop.SetVisible(true);
            Program.items.Add(drop);

            drop = null;
        }

        public void SetItem(Item item)
        {
            drop = item;
            drop.SetVisible(false);
            drop.Holder = this;
        }

        public override void Update()
        {
            if (!dead)
            {
                if (health > 0)
                {
                    // Check collision
                    // up
                    OnCollision(World.GetObjectFromPosition(X, Y - 1));
                    // down
                    OnCollision(World.GetObjectFromPosition(X, Y + 1));
                    // left
                    OnCollision(World.GetObjectFromPosition(X - 1, Y));
                    // right
                    OnCollision(World.GetObjectFromPosition(X + 1, Y));
                }
                else
                {
                    Program.player.Gold += Program.random.Next(0, 11);
                    dead = true;
                }
            }

            if (dead && drop != null)
            {
                DropItem();
            }

            if (!Program.animating && attack)
            {
                attack = false;
            }

            base.Update();
        }

        public override void OnCollision(Object collidedObject)
        {
            if (collidedObject != null)
            {
                switch (collidedObject.GetSprite())
                {
                    case Constants.PLAYER:
                        if (!attack && !Program.animating)
                        {
                            Program.player.Damaged(damage, this);
                            attack = true;
                        }
                        break;

                    default:
                        break;
                }
            }
        }
    }
}

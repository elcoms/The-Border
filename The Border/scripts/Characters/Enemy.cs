using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Border.scripts
{
    class Enemy : Character
    {
        protected Item drop;

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

        public override void Update()
        {
            if (dead && drop != null)
            {
                DropItem();
            }
        }
    }
}

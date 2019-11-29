using System;

namespace The_Border.scripts
{
    class Player : Character
    {
        private int level, gold;
        private Inventory inventory = new Inventory();

        public Player()
        {
            X = 53;
            Y = 13;
            health = 100;
            damage = 10;
            level = 1;
            gold = 0;
            sprite = Constants.PLAYER;
        }

        public override void Render()
        {
            base.Render();
        }

        public Inventory GetInventory() { return inventory; }
    }
}

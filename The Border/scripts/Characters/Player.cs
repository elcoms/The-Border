﻿using System;

namespace The_Border.scripts
{
    class Player : Character
    {
        private int level, gold;
        private Inventory inventory = new Inventory();

        public Player()
        {
            X = 63;
            Y = 17;
            health = 100;
            damage = 10;
            level = 1;
            gold = 0;
            sprite = Constants.PLAYER;
        }

        public override void Render()
        {
            inventory.Render();

            base.Render();
        }

        public Inventory GetInventory() { return inventory; }
    }
}

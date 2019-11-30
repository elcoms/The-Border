using System;

namespace The_Border.scripts
{
    class Player : Character
    {
        private int level = 0, gold = 0;
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

        public void RenderStats()
        {
            Console.SetCursorPosition(Constants.STATS_X, Constants.STATS_Y);
            Console.Write("Level: " + level);
            Console.SetCursorPosition(Constants.STATS_X, Constants.STATS_Y + 1);
            Console.Write("Health: " + health);
            Console.SetCursorPosition(Constants.STATS_X, Constants.STATS_Y + 2);
            Console.Write("Gold: " + gold);
        }

        public Inventory GetInventory() { return inventory; }
    }
}

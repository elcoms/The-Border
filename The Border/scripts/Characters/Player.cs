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

        public override void Update()
        {
            base.Update();
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

        public override void OnCollision(int x, int y, char collision)
        {
            switch (collision)
            {
                case Constants.ENEMY:
                    foreach (Enemy enemy in Program.enemies)
                    {
                        if (enemy.X == x && enemy.Y == y)
                        {
                            // player attack enemy
                            enemy.Damaged(damage, this);
                        }
                    }
                    break;

                case Constants.KEY:

                    for (int i = 0; i < Program.items.Count; i++)
                    {
                        if (Program.items[i].X == x && Program.items[i].Y == y)
                        {
                            // put item into inventory if possible
                            inventory.AddItem(Program.items[i]);
                        }
                    }

                    break;

                case Constants.DOOR_COLLISION:
                    foreach (Door door in Program.doors)
                    {
                        if (door.X == x && door.Y == y)
                        {
                            door.OnCollision(this);
                        }
                    }
                    break;

                case Constants.SPACE:
                    SetPosition(x, y);
                    break;

                default:
                    break;
            }
        }

        public Inventory GetInventory() { return inventory; }
    }
}

using System;
using System.Threading;

namespace The_Border.scripts
{
    class Player : Character
    {
        public int Level { get; set; }
        public int Gold { get; set; }

        private Inventory inventory = new Inventory();

        public Player()
        {
            X = 53;
            Y = 13;
            health = 100;
            damage = 10;
            Level = 1;
            Gold = 0;
            sprite = Constants.PLAYER;
        }

        public override void Render()
        {
            if (dead)
            {
                Console.BackgroundColor = ConsoleColor.DarkRed;
                Program.Log(Level > 4 ? "The Man tried his best but The Border was too overwhelming."
                    : "The Man could not even get close to The Border");

                Console.BackgroundColor = Constants.BACKGROUND_COLOR;
                Thread.Sleep(Constants.GAMEOVER_PAUSE_TIME);
            }

            base.Render();
        }

        public override void Update()
        {
            base.Update();
        }

        public void RenderStats()
        {
            Console.SetCursorPosition(Constants.STATS_X, Constants.STATS_Y);
            Console.Write("Level: " + Level + "   ");

            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(Constants.STATS_X, Constants.STATS_Y + 1);
            Console.Write("Health: " + health + "   ");

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition(Constants.STATS_X, Constants.STATS_Y + 2);
            Console.Write("Gold: " + Gold + "   ");

            Console.ForegroundColor = Constants.FOREGROUND_COLOR;
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

                case Constants.WIN_TRIGGER:
                    Program.win = true;
                    break;

                default:
                    break;
            }
        }

        public Inventory GetInventory() { return inventory; }
    }
}

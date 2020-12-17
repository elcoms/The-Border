using System;
using System.Threading;

namespace TheEndlessBorder.scripts
{
    class Player : Character
    {
        public int Level { get; set; }
        public int Gold { get; set; }

        private Inventory inventory = new Inventory();

        public Player()
        {
            health = 100;
            damage = 10;
            Level = 1;
            Gold = 0;
            sprite = Constants.PLAYER;
            isLit = true;
            objectInBackground = new Object(X, Y, Constants.SPACE);
        }

        public Player(int xPos, int yPos, int hp, int attackPower, int level, int gold, char character, Object backgroundObject) : base(xPos, yPos, hp, attackPower, character, backgroundObject)
        {
            Level = level;
            Gold = gold;
        }

        public override void Render()
        {
            if (dead)
            {
                Console.BackgroundColor = ConsoleColor.DarkRed;
                Program.Log(Level > 4 ? "The Man tried his best but The Border was too overwhelming."
                    : "The Man could not even get close to The Border");

                Console.BackgroundColor = Constants.BACKGROUND_COLOR;
            }

            base.Render();

            if (dead)
                Thread.Sleep(Constants.GAMEOVER_PAUSE_TIME);
        }

        public void RenderStats()
        {
            Console.SetCursorPosition(Constants.STATS_X + 4, Constants.STATS_Y - 2);
            Console.Write(Program.god ? "[GOD]" : "Man  ");

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

        public override void OnCollision(Object collidedObject)
        {
            switch (collidedObject.GetSprite())
            {
                case Constants.ENEMY:
                case Constants.ENEMY_PATROL:
                    // player attack enemy
                    if (collidedObject is Enemy)
                    {
                        (collidedObject as Enemy).Damaged(damage, this);
                    }
                    break;

                case Constants.APPLE:
                case Constants.KEY:
                case Constants.TORCH:

                    if (collidedObject is Item)
                    {
                        Item item = collidedObject as Item;
                        item.OnPickUp(this);
                    }
                    break;

                case Constants.DOOR_HORIZONTAL:
                case Constants.DOOR_VERTICAL:

                    if (collidedObject is Door)
                    {
                        (collidedObject as Door).OnCollision(this);
                    }
                    break;

                case Constants.FLOOR:

                    if (collidedObject.RoomNo != RoomNo)
                    {
                        World.DarkenRoom(RoomNo);
                        World.LightUpRoom(collidedObject.RoomNo);
                        RoomNo = collidedObject.RoomNo;
                    }

                    SetPosition(collidedObject.X, collidedObject.Y);
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

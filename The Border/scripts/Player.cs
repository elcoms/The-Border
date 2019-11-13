using System;

namespace The_Border.scripts
{
    class Player : Character
    {
        private int level, gold;

        public Player()
        {
            X = 5;
            Y = 5;
            health = 100;
            attack = 10;
            level = 1;
            gold = 0;
        }

        public override void Render()
        {
            Console.SetCursorPosition(X, Y);
            Console.Write(Constants.PLAYER);
        }
    }
}

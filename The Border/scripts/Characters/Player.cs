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
            damage = 10;
            level = 1;
            gold = 0;
            sprite = Constants.PLAYER;
        }
    }
}

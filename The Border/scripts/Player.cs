﻿using System;

namespace The_Border.scripts
{
    class Player
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        private int level, gold, health;

        public Player()
        {
            X = 5;
            Y = 5;
            level = 1;
            gold = 0;
            health = 100;
        }

        public void Render()
        {
            Console.SetCursorPosition(X, Y);
            Console.Write(Constants.PLAYER);
        }

        public void Damage(int amt)
        {

        }

        public void Heal(int amt)
        {
            health += amt;
        }

        // increase x and y by given amount if it doesn't go below 0
        public void Move(int xAmt, int yAmt)
        {
            X = (X + xAmt) >= 0 ? (X + xAmt) : X;
            Y = (Y + yAmt) >= 0 ? (Y + yAmt) : Y;
        }
    }
}

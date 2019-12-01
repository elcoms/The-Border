﻿using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace The_Border.scripts
{
    class Character : Object
    {
        protected int health = 1, damage = 1;
        protected bool dead, attacked, horizontalAttack;

        public override void Render()
        {
            // if attacked, show attack graphic
            if (attacked)
            {
                Console.Write(horizontalAttack ? Constants.HORIZONTAL_ATTACK : Constants.VERTICAL_ATTACK);
            }
            else
            {
                if (!dead)
                {
                    // render if alive
                    if (health > 0)
                        Console.Write(sprite);
                }
                else
                {
                    Console.Write(Constants.DEAD);
                }
            }
        }

        public override void Update()
        {
            if (!Program.animating && attacked)
            {
                attacked = false;
            }

            if (health <= 0)
                dead = true;

            base.Update();
        }

        public virtual void Damaged(int amount, Object obj)
        {
            if (!dead)
            {
                // determine if the attack by object is next to this character or above/below
                horizontalAttack = obj.Y == Y ? true : false;

                // suspend the console for a short amt of time before changing back
                if (!Program.animating)
                {
                    Program.animationTimer.Start();
                    Program.animating = true;
                }

                attacked = true;
                health -= amount;
            }
        }

        public virtual void Heal(int amount)
        {
            health += amount;
        }

        public virtual int GetDamage() { return damage; }
        public virtual bool Attacked() { return attacked; }
        public virtual bool Dead() { return dead; }

        public virtual void OnCollision(int x, int y, char collision) { }
    }
}

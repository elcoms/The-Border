using System;
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
            // if attacked, show attack graphic depending on direction for a short amt of time
            if (attacked)
            {
                Console.Write(horizontalAttack ? Constants.HORIZONTAL_ATTACK : Constants.VERTICAL_ATTACK);

                // suspend the console for a short amt of time before changing back
                if (!Program.animationTimer.IsRunning)
                    Program.animationTimer.Start();
                else
                {
                    if (Program.animationTimer.ElapsedMilliseconds > Constants.ATTACK_ANIM_TIME)
                    {
                        Program.animationTimer.Reset();
                        attacked = false;
                    }
                }
            }

            if (!attacked)
            {
                if (!dead)
                {
                    // render if alive
                    if (health > 0)
                        Console.Write(sprite);
                    // render dead body if not, update world data to be walkable
                    else
                    {
                        Console.Write(Constants.DEAD);
                        World.UpdateWorldData(X, Y, Constants.SPACE);
                        dead = true;
                    }
                }
            }
        }

        public virtual void Damaged(int amount, Object obj)
        {
            // determine if the attack by object is next to this character or above/below
            horizontalAttack = obj.Y == Y ? true : false;
            attacked = true;
            health -= amount;
        }

        public virtual void Heal(int amount)
        {
            health += amount;
        }

        public virtual int GetDamage()
        {
            return damage;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Border.scripts
{
    class Character : Object
    {
        protected int health, attack;
        protected bool dead;
        
        public virtual void ReduceHealth(int amount)
        {
            health -= amount;
        }

        public virtual void Heal(int amount)
        {
            health += amount;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Border.scripts
{
    class Object : IObject
    {
        public int X { get; set; }
        public int Y { get; set; }

        protected char sprite = '?';

        public virtual void Render() { }

        // increase x and y by given amount if it doesn't go below 0
        public virtual void Move(int xAmount, int yAmount)
        {
            X = (X + xAmount) >= 0 ? (X + xAmount) : X;
            Y = (Y + yAmount) >= 0 ? (Y + yAmount) : Y;
        }

        // Assign the x and y positions based on given parameters
        public virtual void SetPosition(int xPos, int yPos)
        {
            X = xPos >= 0 ? xPos : X;
            Y = yPos >= 0 ? yPos : Y;
        }
    }
}

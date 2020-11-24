using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheEndlessBorder.scripts
{
    class Object : IObject
    {
        public int X { get; set; }
        public int Y { get; set; }

        protected char sprite = Constants.UNKNOWN;

        public Object() { }

        public Object(int x, int y, char objectSprite)
        {
            X = x;
            Y = y;
            sprite = objectSprite;
        }

        public virtual void Render() 
        {
            Console.Write(sprite);
        }

        public virtual void Update() { }

        // increase x and y by given amount if it doesn't go below 0
        public virtual void Move(int xAmount, int yAmount)
        {
            // remove current position from data
            World.UpdateWorldObjects(new Object(X, Y, Constants.SPACE));
            
            X = (X + xAmount) >= 0 ? (X + xAmount) : X;
            Y = (Y + yAmount) >= 0 ? (Y + yAmount) : Y;

            // update new position in data
            World.UpdateWorldObjects(this);
        }

        // Assign the x and y positions based on given parameters
        public virtual void SetPosition(int xPos, int yPos)
        {
            // remove current position from data
            World.UpdateWorldObjects(new Object(X, Y, Constants.FLOOR));

            X = xPos >= 0 ? xPos : X;
            Y = yPos >= 0 ? yPos : Y;

            // update new position in data
            World.UpdateWorldObjects(this);
        }

        // Getter/Setters
        public char GetSprite() { return sprite; }
    }
}

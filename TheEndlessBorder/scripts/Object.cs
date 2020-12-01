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
        private Object objectInBackground;

        protected char sprite = Constants.UNKNOWN;

        public Object() { }

        public Object(int x, int y, char objectSprite)
        {
            X = x;
            Y = y;
            sprite = objectSprite;
            
            if (sprite != Constants.SPACE)
                objectInBackground = new Object(X, Y, Constants.SPACE);
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
            // remove current position from data and replace with object in background
            if (objectInBackground != null)
                World.UpdateWorldObjects(objectInBackground);

            X = xPos >= 0 ? xPos : X;
            Y = yPos >= 0 ? yPos : Y;

            // save new position's object in background
            objectInBackground = World.GetObjectFromPosition(X, Y);

            // update new position in data
            World.UpdateWorldObjects(this);
        }

        // Modify positions directly without changing anything else
        public virtual void SetPositionDirectly(int xPos, int yPos)
        {
            X = xPos >= 0 ? xPos : X;
            Y = yPos >= 0 ? yPos : Y;
        }

        // Getter/Setters
        public char GetSprite() { return sprite; }
    }
}

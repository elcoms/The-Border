using System;
using System.Collections.Generic;
using System.Text;

namespace TheEndlessBorder.scripts
{
    class Torch : Item
    {
        bool isUsed = false;

        public Torch(int x, int y, char itemSprite, ConsoleColor itemColor, ConsoleColor darkColor, string itemName)
        {
            X = x;
            Y = y;
            sprite = itemSprite;
            color = itemColor;
            unlitColor = darkColor;
            isLit = true;
            Name = itemName;
        }

        public override void Use(Player player)
        {
            // place the torch in world
            player.GetInventory().DropItem(this, player.X, player.Y);

            RoomNo = player.RoomNo;
            World.SaveRoom(RoomNo);

            isUsed = true;
            Program.Log(Name + " lits the room so bright, it will never disappear now.");
        }

        public override void OnDrop()
        {
            isUsed = false;
            objectInBackground = World.GetObjectFromPosition(X, Y);
        }

        public override void Render()
        {
            if (isVisible)
            {
                Console.ForegroundColor = isUsed ? color : unlitColor;
                Console.Write(sprite);
                Console.ForegroundColor = Constants.FOREGROUND_COLOR;
            }
        }

        public override void OnPickUp(Player player)
        {
            World.RemoveRoom(RoomNo);
            isUsed = false;
            player.GetInventory().AddItem(this);
        }
    }
}

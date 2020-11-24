using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheEndlessBorder.scripts
{
    class Inventory
    {
        Item[] items = new Item[5];

        // List items
        public void Render()
        {
            Console.SetCursorPosition(Constants.INVENTORY_X, Constants.INVENTORY_Y);

            // write drop header if drop key pressed
            if (Program.dropKeyPressed)
            {
                // new string to clear spaces after header
                Console.Write("[DROP]" + new string(' ', Constants.WINDOW_WIDTH - Constants.INVENTORY_X - 17));
                Console.SetCursorPosition(Constants.INVENTORY_X, Constants.INVENTORY_Y + 1);
            }

            for (int i = 0; i < items.Length; ++i)
            {
                Console.Write(i+1 + ". ");

                if (items[i] != null)
                {
                    Console.ForegroundColor = items[i].getColor();
                    Console.Write(items[i].Name);
                    Console.ForegroundColor = Constants.FOREGROUND_COLOR;
                }
                // print empty spaces if no item
                else
                {
                    Console.Write(new string(' ', Constants.WINDOW_WIDTH - Constants.INVENTORY_X - 10));
                }

                // set cursor to next line in the interface; increase 1 more line if drop key is pressed
                Console.SetCursorPosition(Constants.INVENTORY_X, Constants.INVENTORY_Y + i + 1 +
                    (Program.dropKeyPressed ? 1 : 0));
            }

            // Clear next line
            Console.Write(new string(' ', Constants.WINDOW_WIDTH - Constants.INVENTORY_X - 10));
        }

        public bool AddItem(Item item)
        {
            bool itemAdded = false;

            // check if inventory got space
            for (int i = 0; i < items.Length; i++)
            {
                // if got space, add item to inventory
                if (items[i] == null)
                {
                    items[i] = item;
                    item.SetVisible(false);
                    itemAdded = true;

                    if (item.Holder != null)
                        item.Holder.SetPosition(item.X, item.Y);
                    else
                    {
                        World.UpdateWorldObjects(new Object(item.X, item.Y, Constants.SPACE));
                    }
                    break;
                }
            }

            // dialogue for adding item successfully/unsuccessfully
            if (itemAdded)
            {
                Program.Log("The Man found " + item.Name + " to accompany him.");
            }
            else
            {
                Program.Log("The Man can't hold many things. The Man has to learn to let go.");
            }

            return itemAdded;
        }

        public void DropItem(int num, int x, int y)
        {
            num -= 1;

            if (items[num] != null)
            {
                Program.Log("The Man left " + items[num].Name + " behind.");
                
                // check for space to drop item
                if (World.GetObjectFromPosition(x, y - 1).GetSprite() == Constants.SPACE)
                    items[num].SetPosition(x, y - 1);
                else if (World.GetObjectFromPosition(x, y + 1).GetSprite() == Constants.SPACE)
                    items[num].SetPosition(x, y + 1);
                else if (World.GetObjectFromPosition(x + 1, y).GetSprite() == Constants.SPACE)
                    items[num].SetPosition(x + 1, y);
                else if (World.GetObjectFromPosition(x - 1, y).GetSprite() == Constants.SPACE)
                    items[num].SetPosition(x - 1, y);
                else
                    items[num].SetPosition(x, y);

                items[num].SetVisible(true);
                items[num] = null;
            }
            else
            {
                Program.Log("There was nothing there that The Man could grab.");
            }
        }

        public void UseItem(int num)
        {
            num -= 1;

            if (items[num] != null)
            {
                items[num].Use(Program.player);
            }
            else
            {
                Program.Log("There was nothing there that The Man could grab.");
            }
        }

        public void RemoveItem(Item item)
        {
            if (item != null)
            {
                for (int i = 0; i < items.Length; i++)
                {
                    if (items[i] == item)
                    {
                        items[i] = null;
                        break;
                    }
                }
            }
        }

        public Item[] GetItems() { return items; }
    }
}

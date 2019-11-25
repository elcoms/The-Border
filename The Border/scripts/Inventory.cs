using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Border.scripts
{
    class Inventory
    {
        Item[] items = new Item[5];

        // List items
        public void Render()
        {
            Console.SetCursorPosition(Constants.INVENTORY_X, Constants.INVENTORY_Y);

            for (int i = 0; i < items.Length; ++i)
            {
                Console.Write(i+1 + ". ");

                if (items[i] != null)
                {
                    Console.Write(items[i].Name);
                }
                // print empty spaces if no item
                else
                {
                    Console.Write(new string(' ', Console.WindowWidth - Constants.INVENTORY_X - 5));
                }

                Console.SetCursorPosition(Constants.INVENTORY_X, Constants.INVENTORY_Y + i + 1);
            }
        }

        public bool AddItem(Item item)
        {
            bool itemAdded = false;

            for (int i = 0; i < items.Length; i++)
            {
                if (items[i] == null)
                {
                    items[i] = item;
                    item.SetVisible(false);
                    itemAdded = true;
                    break;
                }
            }

            if (itemAdded)
            {
                Program.Log("Picked up " + item.Name);
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
                
                World.UpdateWorldData(x, y, items[num].getSprite());
                items[num].SetPosition(x, y);
                items[num].SetVisible(true);
                items[num] = null;
            }
            else
            {
                Program.Log("There was nothing there that The Man could grab.");
            }
        }

        public void RemoveItem(int num)
        {
            num -= 1;

            if (items[num] != null)
            {
                items[num] = null;
            }
        }

        public Item[] GetItems() { return items; }
    }
}

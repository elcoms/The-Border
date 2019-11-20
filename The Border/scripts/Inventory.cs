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

        public void Render()
        {
            Console.SetCursorPosition(Constants.INVENTORY_X, Constants.INVENTORY_Y);

            for (int i = 1; i < items.Length - 1; i++)
            {
                Console.Write(i + ". ");

                if (items[i] != null)
                {
                    Console.Write(items[i].Name);
                }

                Console.SetCursorPosition(Constants.INVENTORY_X, Constants.INVENTORY_Y + i);
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

        public void DropItem(Item item)
        {

        }
    }
}

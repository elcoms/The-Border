﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheEndlessBorder.scripts
{
    class HealItem : Item
    {
        private int healAmount;
        private string healLog;

        public HealItem(int x, int y, int healAmt, char itemSprite, ConsoleColor itemColor, ConsoleColor darkColor, string itemName, string log)
        {
            X = x;
            Y = y;
            healAmount = healAmt;
            sprite = itemSprite;
            color = itemColor;
            unlitColor = darkColor;
            isLit = true;
            Name = itemName;
            healLog = log;
        }

        public override void Use(Player player)
        {
            player.Heal(healAmount);
            player.GetInventory().RemoveItem(this);

            Program.Log(healLog);
        }
    }
}

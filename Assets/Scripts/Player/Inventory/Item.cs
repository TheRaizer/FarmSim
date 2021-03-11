﻿using UnityEngine;

namespace FarmSim.Player 
{
    /// <class name="Item">
    ///     <summary>
    ///         Class that represents anything that can be stored in the players inventory.
    ///     </summary>
    /// </class>
    public class Item
    {
        public readonly ItemType itemType;
        public bool CanSubtract => Amt > 0;

        public int Amt { get; private set; }

        /// <param name="startAmt">The amount to initialize the item with.</param>
        /// <param name="_itemType">Acts as an enum as there should be only a single instance of a Scriptable Object.</param>
        public Item(int startAmt, ItemType _itemType)
        {
            Amt = startAmt;
            itemType = _itemType;
        }

        public void AddToAmt(int amt)
        {
            Amt += amt;
        }

        public void SubtractFromAmt(int amt)
        {
            Amt -= amt;
            if(Amt < 0)
            {
                Debug.LogError("Amt has dipped below zero that should not occur. Use the CanSubtract bool to make sure this doesn't occur");
            }
        }
    }
}

using UnityEngine;

namespace FarmSim.Player 
{
    public class Item
    {
        public readonly ItemType itemType;
        public int Amt { get; private set; }

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
        }
    }
}

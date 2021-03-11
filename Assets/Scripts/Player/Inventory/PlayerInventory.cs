using System.Collections.Generic;
using UnityEngine;

namespace FarmSim.Player
{
    public class PlayerInventory : MonoBehaviour
    {
        private readonly Dictionary<ItemType, Item> inventory = new Dictionary<ItemType, Item>();

        public void AddToInventory(ItemType itemType, int amt)
        {
            if (inventory.ContainsKey(itemType))
            {
                inventory[itemType].AddToAmt(amt);
            }
            else
            {
                inventory.Add(itemType, new Item(amt, itemType));
            }
            Debug.Log("item amount: " + inventory[itemType].Amt);
        }

        public void TakeFromInventory(ItemType itemType, int amt)
        {
            if (inventory.ContainsKey(itemType))
            {
                inventory[itemType].SubtractFromAmt(amt);
            }
            else
            {
                Debug.Log($"Inventory does not yet have item of type {itemType}");
            }
        }
    }
}

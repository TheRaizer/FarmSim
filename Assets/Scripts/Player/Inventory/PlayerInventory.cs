using System.Collections.Generic;
using UnityEngine;

namespace FarmSim.Player
{
    /// <class name="PlayerInventory">
    ///     <summary>
    ///         Class that contains and manages the player's items.
    ///     </summary>
    /// </class>
    public class PlayerInventory : MonoBehaviour
    {
        //TEST CODE
        [SerializeField] private ItemType tomSeed;

        private readonly Dictionary<ItemType, Item> inventory = new Dictionary<ItemType, Item>();

        private void Awake()
        {
            //TEST CODE
            inventory.Add(tomSeed, new Item(5, tomSeed));
        }

        /// <summary>
        ///     Add an amount of an item or add an entirely new item to the player's inventory.
        /// </summary>
        /// <param name="itemType">The singular instance of a SO that points to an item in the inventory.</param>
        /// <param name="amt">The amount to add to an item.</param>
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

        /// <summary>
        ///     Subtract an amount of an item from the player's inventory.
        /// </summary>
        /// <param name="itemType">The singular instance of a SO that points to an item in the inventory.</param>
        /// <param name="amt">The amount to subtract from an item.</param>
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

        /// <summary>
        ///     Retrieve an Item from the player's inventory.
        /// </summary>
        /// <param name="itemType">The singular instance of a SO that points to an item in the inventory.</param>
        /// <returns>An <see cref="Item"/> or null if the itemType does not point to anything.</returns>
        public Item GetItem(ItemType itemType)
        {
            if (inventory.ContainsKey(itemType))
            {
                return inventory[itemType];
            }
            Debug.Log($"Inventory does not yet have item of type {itemType}");
            return null;
        }
    }
}

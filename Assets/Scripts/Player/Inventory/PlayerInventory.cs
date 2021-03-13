using FarmSim.Serialization;
using System.Collections.Generic;
using UnityEngine;

namespace FarmSim.Player
{
    /// <class name="PlayerInventory">
    ///     <summary>
    ///         Class that contains and manages the player's items.
    ///     </summary>
    /// </class>
    public class PlayerInventory : MonoBehaviour, ISaveable
    {
        // TEST ATTRIBUTES
        [SerializeField] private ItemType potatoSeed;
        [SerializeField] private ItemType tomatoSeed;

        private readonly Dictionary<ItemType, Item> inventory = new Dictionary<ItemType, Item>();

        private void Awake()
        {
            /*AddToInventory(potatoSeed, 5);
            AddToInventory(tomatoSeed, 5);*/

            // we can load on awake because data loading should happen in a different scene
            LoadItems();
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
                if (inventory[itemType].Amt <= 0)
                {
                    Debug.Log($"Not enough of {itemType.ItemName}");
                }
                else
                {
                    if (inventory[itemType].CanSubtract)
                    {
                        inventory[itemType].SubtractFromAmt(amt);
                    }
                }
            }
            else
            {
                Debug.Log($"Inventory does not yet have item of type {itemType.ItemName}");
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

        //TEST CODE
        private void LoadItems()
        {
            // obtain list of itemDatas
            List<ItemData> itemDatas = SaveData.current.playerData.itemDatas;

            // if there is data
            if (itemDatas != null)
            {
                Debug.Log("There are items to load");
                itemDatas.ForEach(itemData =>
                {
                    // Obtain the SO from the data's itemTypeName attribute.
                    ItemType itemType = Resources.Load("SO/" + itemData.itemTypeName) as ItemType;

                    Debug.Log("Item type: " + itemType.ItemName + " || Item amt: " + itemData.amt);

                    // adds the item to the inventory
                    AddToInventory(itemType, itemData.amt);
                });
            }
            else
            {
                Debug.Log("No items to load");
            }
        }

        public void Save()
        {
            // Creates a list of itemDatas
            List<ItemData> itemDatas = new List<ItemData>();

            foreach (KeyValuePair<ItemType, Item> kvp in inventory)
            {
                // get item and corrosponding item type in dict
                Item item = kvp.Value;
                ItemType itemType = kvp.Key;

                // create an itemData object
                ItemData itemData = new ItemData(item.Amt, itemType.name);
                Debug.Log("item type: " + itemData.itemTypeName + " || amt: " + itemData.amt);
                // assign the object to the itemDatas list
                itemDatas.Add(itemData);
            }

            /* assign the itemDatas list to the Serializable SaveData singleton in 
             * order for it to be saved along side the rest of the games data
             */
            SaveData.current.playerData.itemDatas = itemDatas;
            Debug.Log("Save item data");
        }
        /// <summary>
        ///     Saves the players inventory then saves the SaveData class to a file.
        /// </summary>
    }
}

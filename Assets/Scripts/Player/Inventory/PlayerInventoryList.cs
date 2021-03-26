using FarmSim.Serialization;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace FarmSim.Player
{
    /// <class name="PlayerInventoryList">
    ///     <summary>
    ///         Class that contains and manages the player's items.
    ///     </summary>
    /// </class>
    public class PlayerInventoryList : MonoBehaviour, ISavable, ILoadable
    {
        private readonly int maxStorage = 6;
        private InventoryUI inventoryUI;
        private readonly List<Item> inventory = new List<Item>();

        private void Awake()
        {
            inventoryUI = GetComponent<InventoryUI>();
        }

        /// <summary>
        ///     Add an amount of an item or add an entirely new item to the player's inventory.
        /// </summary>
        /// <param name="itemType">The singular instance of a SO that points to an item in the inventory.</param>
        /// <param name="amt">The amount to add to an item.</param>
        public void AddToInventory(ItemType itemType, int amt, bool firstLoad = false)
        {
            // find items that matche and have enough room
            List<Item> validItems = inventory.FindAll(x => (x.itemType == itemType) && (x.Amt < x.itemType.MaxCarryAmt));

            Assert.IsTrue(inventory.Count <= maxStorage);

            if(inventory.Count == maxStorage)
            {
                // Create popup screen for remaining item.
                return;
            }
            // if there arent any matching items to the given item type
            if (validItems.Count <= 0)
            {
                CreateItemsForPossibleOverflow(amt, itemType, false, firstLoad);
            }
            else
            {
                // fill up all the valid matching items with the amt
                int remaining = amt;
                foreach(Item item in validItems)
                {
                    // calculate the amt that would come from adding onto the item.Amt
                    int newAmt = item.Amt + remaining;

                    if(item.Amt < item.itemType.MaxCarryAmt)
                    {
                        int tempAmt = item.Amt;

                        item.AddToAmt(remaining);

                        // (item max carry amount - tempAmt) will give the amt that the remaining filled in.
                        remaining -= item.itemType.MaxCarryAmt - tempAmt;

                        if (remaining <= 0)
                        {
                            return;
                        }
                    }
                }
                // try to add new items
                CreateItemsForPossibleOverflow(remaining, itemType, true, firstLoad);
            }
        }

        private void CreateItemsForPossibleOverflow(int amt, ItemType itemType, bool hadValidItems, bool firstLoad)
        {
            // get the number of items to generate - 1
            int numItemsToGenerate = Mathf.FloorToInt(amt / itemType.MaxCarryAmt);

            // loop through the items to generate
            for (int i = 0; i < numItemsToGenerate; i++)
            {
                // if we can add to the inventory
                if (inventory.Count + 1 <= maxStorage)
                {
                    // add item with max carry amt to the inventory
                    inventory.Add(new Item(itemType.MaxCarryAmt, itemType));
                    // reduce the amt
                    amt -= itemType.MaxCarryAmt;
                }
                else
                {
                    break;
                }
            }
            // add one more item with the remaining amt if we can
            if (inventory.Count + 1 <= maxStorage)
            {
                Item item = new Item(amt, itemType);
                inventory.Add(item);

                if (!hadValidItems && !firstLoad)
                {
                    if (inventoryUI == null)
                    {
                        Debug.LogWarning("inventoryUI is null");
                        return;
                    }
                    inventoryUI.AddImageToSlot(item, inventory.Count - 1);
                }
            }
            else
            {
                Debug.LogWarning($"Remainder is {amt}");
                // pop up for remainder amount of 'amt'
            }
        }

        /// <summary>
        ///     Subtract an amount of an item from the player's inventory.
        /// </summary>
        /// <param name="itemType">The singular instance of a SO that points to an item in the inventory.</param>
        /// <param name="amtToTake">The amount to subtract from an item.</param>
        public Item TakeFromInventory(ItemType itemType, int amtToTake)
        {
            Item item = inventory.FirstOrDefault(x => (x.itemType == itemType) && x.Amt >= amtToTake);

            // only subtract and return an item if it exists and taking from it will not result in a negative amount
            if (item != null && item.Amt - amtToTake >= 0)
            {
                item.SubtractFromAmt(amtToTake);

                if (!item.CanSubtract)
                {
                    if(item.Icon == null)
                        Debug.LogWarning($"Icon on {item} is null");
                    else
                        Destroy(item.Icon.gameObject);
                    inventory.Remove(item);
                }

                return item;
            }
            Debug.Log($"Not enough of {itemType.ItemName}");

            return null;
        }

        public List<Item> FindInstances(ItemType instanceType)
        {
            return inventory.FindAll(x => x.itemType == instanceType);
        }

        public void Load()
        {
            // obtain list of itemDatas
            List<ItemData> itemDatas = SaveData.Current.playerData.itemDatas;

            // if there is data
            if (itemDatas != null)
            {
                Debug.Log("There are items to load");
                itemDatas.ForEach(itemData =>
                {
                // Obtain the SO from the data's itemTypeName attribute.
                ItemType itemType = Resources.Load("SO/" + itemData.itemTypeName) as ItemType;
                if (itemType == null)
                {
                    Debug.LogError("There is no scriptable object at path: " + "SO/" + itemData.itemTypeName);
                }

                Debug.Log("Item type: " + itemType.ItemName + " || Item amt: " + itemData.amt);

                // adds the item to the inventory
                AddToInventory(itemType, itemData.amt, true);
                });
            }
            else
            {
                Debug.Log("No items to load");
            }
            Debug.Log("list loading");
            inventoryUI.InitializeSlots(maxStorage, inventory);
        }

        public void Save()
        {
            // Creates a list of itemDatas
            List<ItemData> itemDatas = new List<ItemData>();

            foreach (Item item in inventory)
            {
                // get item and corrosponding item type in dict
                ItemType itemType = item.itemType;

                // create an itemData object
                ItemData itemData = new ItemData(item.Amt, itemType.name);
                Debug.Log("item type: " + itemData.itemTypeName + " || amt: " + itemData.amt);
                // assign the object to the itemDatas list
                itemDatas.Add(itemData);
            }

            /* assign the itemDatas list to the Serializable SaveData singleton in 
             * order for it to be saved along side the rest of the games data
             */
            SaveData.Current.playerData.itemDatas = itemDatas;
            Debug.Log("Save item data");
        }
    }
}
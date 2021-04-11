using FarmSim.Attributes;
using FarmSim.Serialization;
using FarmSim.Slots;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace FarmSim.Items
{
    /// <class name="PlayerInventoryList">
    ///     <summary>
    ///         Class that contains and manages the player's items.
    ///     </summary>
    /// </class>
    [Savable(true)]
    public class Inventory : MonoBehaviour, ISavable, ILoadable
    {
        private readonly int maxStorage = 6;
        private readonly List<Item> inventory = new List<Item>();
        private InventorySlotsHandler inventoryUI;

        private void Awake()
        {
            inventoryUI = GetComponent<InventorySlotsHandler>();
        }

        /// <summary>
        ///     Add an amount of an item or add an entirely new item to the player's inventory.
        /// </summary>
        /// <param name="itemType">The singular instance of a SO that points to an item in the inventory.</param>
        /// <param name="amt">The amount to add to an item.</param>
        public void AddToInventory(ItemType itemType, int amt, Action onSuccesful = null, Action onFailure = null)
        {
            // find items that match and have enough room
            List<Item> validItems = inventory.FindAll(x => (x.itemType == itemType) && (x.Amt < itemType.MaxCarryAmt));

            if (inventory.Count >= maxStorage && validItems.Count <= 0)
            {
                onFailure?.Invoke();
                return;
            }
            // if there arent any matching items to the given item type
            if (validItems.Count <= 0)
            {
                CreateItemsForPossibleOverflow(amt, itemType);

                onSuccesful?.Invoke();
            }
            else
            {
                // fill up all the valid matching items with the amt
                int remaining = amt;
                foreach (Item item in validItems)
                {
                    // calculate the amt that would come from adding onto the item.Amt
                    int newAmt = item.Amt + remaining;

                    if (item.Amt < item.itemType.MaxCarryAmt)
                    {
                        int tempAmt = item.Amt;

                        item.AddToAmt(remaining);

                        // (item max carry amount - tempAmt) will give the amt that the remaining filled in.
                        remaining -= item.itemType.MaxCarryAmt - tempAmt;

                        if (remaining <= 0)
                        {
                            onSuccesful?.Invoke();
                            return;
                        }
                    }
                }

                // try to add new items
                CreateItemsForPossibleOverflow(remaining, itemType);
            }
            onSuccesful?.Invoke();
        }

        /// <summary>
        ///     Subtract an amount of an item from the player's inventory.
        /// </summary>
        /// <param name="itemType">The singular instance of a SO that points to an item in the inventory.</param>
        /// <param name="amtToTake">The amount to subtract from an item.</param>
        public Item TakeFromInventory(string guid, int amtToTake)
        {
            Item item = inventory.Find(x => (x.guid == guid) && x.Amt >= amtToTake);

            // only subtract and return an item if it exists and taking from it will not result in a negative amount
            if (item != null && item.Amt - amtToTake >= 0)
            {
                item.SubtractFromAmt(amtToTake);

                if (!item.CanSubtract)
                {
                    if (item.Icon == null)
                        Debug.LogWarning($"Icon on {item} is null");
                    else
                        Destroy(item.Icon.gameObject);
                    inventory.Remove(item);
                }

                return item;
            }
            Debug.Log($"Not enough of item with id {guid}");

            return null;
        }

        private void CreateItemsForPossibleOverflow(int amt, ItemType itemType)
        {
            // get the number of items to generate - 1
            int numItemsToGenerate = Mathf.FloorToInt(amt / itemType.MaxCarryAmt);

            // loop through the items to generate
            for (int i = 0; i < numItemsToGenerate; i++)
            {
                // if we can add to the inventory
                if (inventory.Count + 1 <= maxStorage)
                {
                    Item item = new Item(itemType.MaxCarryAmt, itemType, StackItems, DeleteItem);

                    // add item with max carry amt to the inventory
                    inventory.Add(item);
                    AddImage(item);

                    // reduce the amt
                    amt -= itemType.MaxCarryAmt;
                }
                else
                {
                    break;
                }
            }
            // add one more item with the remaining amt if we can
            if (inventory.Count + 1 <= maxStorage && amt > 0)
            {
                Item item = new Item(amt, itemType, StackItems, DeleteItem);

                inventory.Add(item);
                AddImage(item);
            }
            else if (amt > 0)
            {
                Debug.LogWarning($"Remainder is {amt}");
                // drop item with amt
            }
        }

        /// <summary>
        ///     Stacks two items returning the remainder.
        /// </summary>
        /// <param name="item1">The item you will be adding to the stack.</param>
        /// <param name="item2">The item that acts as the stack.</param>
        /// <returns></returns>
        public int StackItems(Item item1, Item item2)
        {
            // if adding item1 to item2 does not create an overflow
            if (item1.Amt + item2.Amt <= item2.itemType.MaxCarryAmt)
            {
                // add item1 amt to item2
                item2.AddToAmt(item1.Amt);
                return 0;
            }

            int remainder = item1.Amt + item2.Amt - item2.itemType.MaxCarryAmt;
            item2.AddToAmt(item2.itemType.MaxCarryAmt - item2.Amt);

            return remainder;
        }

        /// <summary>
        ///     Run this after adding to inventory in order to spawn the correct Image at a slot.
        /// </summary>
        /// <param name="item">The item whose image we will be creating.</param>
        /// <param name="hadValidItems">Whether this item had any valid items.</param>
        /// <param name="firstLoad">Whether this is run when the inventory is loading.</param>
        private void AddImage(Item item)
        {
            if (inventoryUI == null)
            {
                Debug.LogWarning("inventoryUI is null");
                return;
            }
            inventoryUI.AddImageToSlot(item);
        }

        public void DeleteItem(string guid)
        {
            Item item = GetExactItem(guid);
            Destroy(item.Icon.gameObject);
            inventory.Remove(item);
        }

        public bool CanAdd(ItemType itemType)
        {
            List<Item> validItems = inventory.FindAll(x => (x.itemType == itemType) && (x.Amt < itemType.MaxCarryAmt));

            return inventory.Count < maxStorage || validItems.Count > 0;
        }

        public bool Contains(ItemType itemType)
        {
            return inventory.Find(x => x.itemType == itemType) != null;
        }

        public Item GetFirstInstance(ItemType itemType)
        {
            return inventory.Find(x => x.itemType == itemType);
        }

        public Item GetExactItem(string guid)
        {
            return inventory.Find(x => x.guid == guid);
        }

        public List<Item> FindInstances(ItemType instanceType)
        {
            return inventory.FindAll(x => x.itemType == instanceType);
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
                ItemData itemData = new ItemData(item.Amt, itemType.name, item.SlotIndex);
                Debug.Log("item type: " + itemData.itemTypeName + " || amt: " + itemData.amt);
                // assign the object to the itemDatas list
                itemDatas.Add(itemData);
            }

            /* assign the itemDatas list to the Serializable SaveData singleton in 
             * order for it to be saved along side the rest of the games data
             */
            PlayerData.Current.itemDatas = itemDatas;
            Debug.Log("Save item data");
        }

        public void Load()
        {
            // obtain list of itemDatas
            List<ItemData> itemDatas = PlayerData.Current.itemDatas;

            // if there is data
            if (itemDatas != null)
            {
                LoadExistingItems(itemDatas);
            }
            inventoryUI.InitializeUI(maxStorage, inventory);
        }

        private void LoadExistingItems(List<ItemData> itemDatas)
        {
            itemDatas.ForEach(itemData =>
            {
                // Obtain the SO from the data's itemTypeName attribute.
                ItemType itemType = Resources.Load("SO/" + itemData.itemTypeName) as ItemType;
                if (itemType == null)
                {
                    Debug.LogError("There is no scriptable object at path: " + "SO/" + itemData.itemTypeName);
                }

                Debug.Log("Item type: " + itemType.ItemName + " || Item amt: " + itemData.amt);

                // Loads the item into the inventory
                Item item = new Item(itemData.amt, itemType, StackItems, DeleteItem);
                inventory.Add(item);
                item.SlotIndex = itemData.slotIndex;
            });
        }
    }
}
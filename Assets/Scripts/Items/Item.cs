using FarmSim.Slots;
using FarmSim.Utility;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FarmSim.Items
{
    /// <class name="Item">
    ///     <summary>
    ///         Class that represents anything that can be stored in the players inventory.
    ///     </summary>
    /// </class>
    public class Item : ISwappable
    {
        /// <summary>
        ///     The Icon of the item in the inventory
        /// </summary>
        public Image Icon { get; private set; }
        public int Amt { get; private set; }
        public int SlotIndex { get; set; }
        public bool CanSubtract => Amt > 0;

        public SlotsHandler SlotsHandler { get; set; }

        private TextMeshProUGUI TextAmt;

        public readonly string guid;
        public readonly ItemType itemType;
        private readonly Func<Item, Item, int> stackItems;
        private readonly Action<string> deleteItem;

        /// <param name="startAmt">The amount to initialize the item with.</param>
        /// <param name="_itemType">Acts as an enum as there should be only a single instance of a Scriptable Object.</param>
        public Item(int startAmt, ItemType _itemType, Func<Item, Item, int> _stackItems, Action<string> _deleteItem)
        {
            Amt = startAmt;
            itemType = _itemType;
            stackItems = _stackItems;
            deleteItem = _deleteItem;
            guid = Guid.NewGuid().ToString();
        }

        public void AddToAmt(int amt)
        {
            if (amt < 0)
            {
                Debug.LogWarning($"Attempted to add by {amt} from {itemType.ItemName}");
                return;
            }
            Amt += amt;
            Amt = Mathf.Clamp(Amt, 0, itemType.MaxCarryAmt);
            SetTextAmt();
        }

        public void SubtractFromAmt(int amt)
        {
            if (amt < 0)
            {
                Debug.LogWarning($"Attempted to subtract by {amt} from {itemType.ItemName}");
                return;
            }
            Amt -= amt;
            if (Amt < 0)
                Amt = 0;
            SetTextAmt();
        }

        private void SetTextAmt()
        {
            if (TextAmt != null)
            {
                TextAmt.SetText(Amt.ToString());
            }
        }

        /// <summary>
        ///     Spawns the inventory icon into a given slot.
        /// </summary>
        /// <param name="slotIndex">The index of the slot that will parent the icon.</param>
        /// <param name="slotsHandler">Used to pass into the icon's <see cref="SwappablePositionManager"/></param>
        /// <returns>The items icon <see cref="GameObject"/> instance</returns>
        public GameObject SpawnInventoryIcon(int slotIndex, InventorySlotsHandler slotsHandler)
        {
            SlotIndex = slotIndex;
            GameObject itemObj = UnityEngine.Object.Instantiate(itemType.IconPrefab);

            // First child of the icon gameObject must be the text amt
            GameObject textAmt = itemObj.transform.GetChild(0).gameObject;

            if (itemObj.TryGetComponent(out IReferenceGUID guid))
            {
                guid.Guid = this.guid;
            }

            // assign the slot index to the position manager for movement of items
            var positionManager = itemObj.GetComponent<SwappablePositionManager>();
            positionManager.SetSwappable(this);
            SlotsHandler = slotsHandler;

            Icon = itemObj.GetComponent<Image>();

            TextAmt = textAmt.GetComponent<TextMeshProUGUI>();

            SetTextAmt();

            return itemObj;
        }

        /// <summary>
        ///     Stacks the attached item onto the other item and returns true if it was stacked.
        /// </summary>
        /// <param name="other">The item that will act as the stack.</param>
        /// <returns>True if attached item was stacked onto other item, otherwise false.</returns>
        public bool AvoidSwap(ISwappable other)
        {
            // if the other position manager is holding an item
            if(other.GetType() == typeof(Item))
            {
                // cast the swappable to an item
                Item otherItem = (Item)other;

                // if the item types are the same we can potentially stack them
                if (otherItem.itemType == itemType)
                {
                    int remainder = stackItems(this, otherItem);
                    if (remainder == 0)
                    {
                        UnityEngine.Object.Destroy(Icon.gameObject);
                        return true;
                    }
                    else
                    {
                        SubtractFromAmt(int.MaxValue);
                        AddToAmt(remainder);
                        return true;
                    }
                }
                return false;
            }
            Debug.Log("Avoid");
            return true;
        }

        public void OnDestroy()
        {
            deleteItem(guid);
        }

        public override string ToString()
        {
            return itemType + " || Amount: " + Amt;
        }
    }
}

using FarmSim.Slots;
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
        ///     The specific slot this items icon lies in.
        /// </summary>
        public int SlotIndex { get; set; }

        /// <summary>
        ///     The slots area this items icon is contained in.
        /// </summary>
        public SlotsHandler SlotsHandler { get; set; }

        /// <summary>
        ///     When item is being destroyed remove attached placeable if its GUID matches this items.
        ///     
        ///     <para>
        ///         Assigned only when the placeable is spawned.
        ///     </para>
        /// </summary>
        public Action<string> RemoveAttachedPlaceableIfMatching { private get; set; }

        /// <summary>
        ///     The Icon of the item in the inventory
        /// </summary>
        public Image Icon { get; private set; }
        public int Amt { get; private set; }

        public bool CanSubtract => Amt > 0;

        private TextMeshProUGUI TextAmt;

        public readonly string guid;
        public readonly ItemType itemType;
        private readonly Func<Item, Item, int> stackItems;
        private readonly Action<string> deleteItem;

        private const int TEXT_CHILD_IDX = 1;

        /// <param name="startAmt">The amount to initialize the item with.</param>
        /// <param name="_itemType">Acts as an enum as there should be only a single instance of a Scriptable Object.</param>
        /// /// <param name="_stackItems">A function that attempts to stack two items together and returns the remainder.</param>
        /// /// <param name="_deleteItem">A void function that deletes the item from the inventory.</param>
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

            // Second child of the icon gameObject must be the text amt
            GameObject textAmt = itemObj.transform.GetChild(TEXT_CHILD_IDX).gameObject;

            if (itemObj.TryGetComponent(out IItemRefsGUID guid))
            {
                // assign the reference guid to be this items guid
                guid.ItemGuid = this.guid;
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
            // if the other swappable is an item
            if (other.GetType() == typeof(Item))
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
            RemoveAttachedPlaceableIfMatching?.Invoke(guid);
        }

        public override string ToString()
        {
            return itemType + " || Amount: " + Amt;
        }
    }
}

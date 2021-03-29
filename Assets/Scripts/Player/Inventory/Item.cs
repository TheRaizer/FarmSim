using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using FarmSim.Utility;

namespace FarmSim.Player 
{
    /// <class name="Item">
    ///     <summary>
    ///         Class that represents anything that can be stored in the players inventory.
    ///     </summary>
    /// </class>
    public class Item
    {

        /// <summary>
        ///     The Icon of the item in the inventory
        /// </summary>
        public Image Icon { get; private set; }
        public int Amt { get; private set; }
        public int SlotIndex { get; set; }

        /// <summary>
        ///     Given to other objects through the <see cref="InventoryUI.SpawnImage(Item, Image)"/> method.
        /// </summary>
        public readonly string guid;
        public readonly ItemType itemType;
        public bool CanSubtract => Amt > 0;
        private TextMeshProUGUI TextAmt;

        /// <param name="startAmt">The amount to initialize the item with.</param>
        /// <param name="_itemType">Acts as an enum as there should be only a single instance of a Scriptable Object.</param>
        public Item(int startAmt, ItemType _itemType)
        {
            Amt = startAmt;
            itemType = _itemType;
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
            if(Amt < 0)
                Amt = 0;
            SetTextAmt();
        }

        private void SetTextAmt()
        {
            if(TextAmt != null)
            {
                TextAmt.SetText(Amt.ToString());
            }
        }

        public GameObject SpawnImageObject(int slotIndex)
        {
            SlotIndex = slotIndex;
            GameObject itemObj = UnityEngine.Object.Instantiate(itemType.IconPrefab);
            GameObject textAmt = itemObj.transform.GetChild(0).gameObject;

            if (itemObj.TryGetComponent(out IReferenceGUID guid))
            {
                guid.Guid = this.guid;
            }

            // assign the slot index to the position manager for movement of items
            var positionManager = itemObj.GetComponent<ItemPositionManager>();
            positionManager.Item = this;

            Icon = itemObj.GetComponent<Image>();

            TextAmt = textAmt.GetComponent<TextMeshProUGUI>();

            SetTextAmt();

            return itemObj;
        }
    }
}

using UnityEngine;
using UnityEngine.UI;
using FarmSim.Utility;

namespace FarmSim.Player 
{
    /// <class name="ItemMovementManager">
    ///     <summary>
    ///         Manages the movement of items from one slot to another.
    ///     </summary>
    /// </class>
    public class ItemMovementManager : MonoBehaviour
    {
        [SerializeField] private Transform itemMovementParent;
        public int AttachedItemSlotIndex { private get; set; }

        private InventoryUI inventoryUI;
        private PlayerInventoryList inventory;
        private Canvas canvas;

        private Item attachedItem;

        private void Awake()
        {
            canvas = FindObjectOfType<Canvas>();
            inventoryUI = GetComponent<InventoryUI>();
            inventory = GetComponent<PlayerInventoryList>();
        }

        private void Update()
        {
            MoveAttachedImgToMouse();
        }

        /// <summary>
        ///     Swaps the positions of 1-2 item <see cref="Image"/>'s in the inventory UI.
        /// </summary>
        /// <param name="otherSlotIndex">The slot index of the other item</param>
        /// <param name="otherItem">The other item that will be swapped with the <see cref="attachedItem"/></param>
        public void SwapPositions(int otherSlotIndex, ItemPositionManager otherItem)
        {
            if (otherItem != null)
            {
                if (StackItems(otherItem))
                    return;
                otherItem.Item.SlotIndex = AttachedItemSlotIndex;
                inventoryUI.MoveImageToSlot(otherItem.gameObject, AttachedItemSlotIndex);
            }

            // swap places
            attachedItem.SlotIndex = otherSlotIndex;
            inventoryUI.MoveImageToSlot(attachedItem.Icon.gameObject, otherSlotIndex);

            // make it clickable again
            attachedItem.Icon.raycastTarget = true;
            attachedItem = null;
        }

        public void SetAttachedItem(Item _attachedItem)
        {
            if (_attachedItem == null)
                return;

            attachedItem = _attachedItem;

            // set a new parent so it appears over all slots
            attachedItem.Icon.transform.SetParent(itemMovementParent);

            // make sure it isn't clickable
            attachedItem.Icon.raycastTarget = false;
        }

        public void DestroyAttachedItem()
        {
            if (attachedItem != null)
            {
                inventory.DeleteItem(attachedItem.guid);
                attachedItem = null;
            }
        }

        public bool HasAttachedItem()
        {
            return attachedItem != null;
        }

        /// <summary>
        ///     Stacks the attached item onto the other item and returns true if it was stacked.
        /// </summary>
        /// <param name="otherItem">The item that will act as the stack.</param>
        /// <returns>True if attached item was stacked onto other item, otherwise false.</returns>
        private bool StackItems(ItemPositionManager otherItem)
        {
            if (otherItem.Item.itemType == attachedItem.itemType)
            {
                int remainder = inventory.StackItems(attachedItem, otherItem.Item);
                if (remainder == 0)
                {
                    Destroy(attachedItem.Icon.gameObject);
                    return true;
                }
                else
                {
                    attachedItem.SubtractFromAmt(int.MaxValue);
                    attachedItem.AddToAmt(remainder);
                    return true;
                }
            }
            return false;
        }

        private void MoveAttachedImgToMouse()
        {
            if (attachedItem != null)
            {
                attachedItem.Icon.rectTransform.SetToMouse(canvas);
            }
        }
    }
}
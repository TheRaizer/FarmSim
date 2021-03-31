using UnityEngine;
using UnityEngine.UI;
using FarmSim.Utility;
using UnityEngine.Assertions;

namespace FarmSim.Items 
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
        private Inventory inventory;
        private Canvas canvas;

        private Item attachedItem;

        private void Awake()
        {
            canvas = FindObjectOfType<Canvas>();
            inventory = GetComponent<Inventory>();
        }

        private void Update()
        {
            MoveAttachedImgToMouse();
        }

        /// <summary>
        ///     Swaps the positions of 1-2 item <see cref="Image"/>'s in the inventory UI.
        ///     <remarks>
        ///         If otherItem is null a slotHandler must be given.
        ///     </remarks>
        /// </summary>
        /// <param name="otherSlotIndex">The slot index of the other item</param>
        /// <param name="otherItem">The other item that will be swapped with the <see cref="attachedItem"/></param>
        /// <param name="slotHandler">Used to move the attached item to the correct empty slot</param>
        public void SwapPositions(int otherSlotIndex, ItemPositionManager otherItem, ItemSlotsHandler slotHandler=null)
        {
            ItemPositionManager attachedItemPosManager = attachedItem.Icon.GetComponent<ItemPositionManager>();

            if (otherItem != null)
            {
                if (StackItems(otherItem))
                    return;
                // move other item to the attached items slot
                otherItem.Item.SlotIndex = AttachedItemSlotIndex;
                attachedItemPosManager.SlotsHandler.MoveImageToSlot(otherItem.gameObject, AttachedItemSlotIndex);

                // move attached item to the other items slot
                attachedItem.SlotIndex = otherSlotIndex;
                otherItem.SlotsHandler.MoveImageToSlot(attachedItem.Icon.gameObject, otherSlotIndex);

                // swap which slot handler they will reference
                ItemSlotsHandler temp = attachedItemPosManager.SlotsHandler;
                attachedItemPosManager.SlotsHandler = otherItem.SlotsHandler;
                otherItem.SlotsHandler = temp;
            }
            else
            {
                Assert.IsNotNull(slotHandler);
                // move attached item to the empty slot
                attachedItem.SlotIndex = otherSlotIndex;
                slotHandler.MoveImageToSlot(attachedItem.Icon.gameObject, otherSlotIndex);


                // assign the slot handler of the empty slot to the attached item.
                attachedItemPosManager.SlotsHandler = slotHandler;
            }


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
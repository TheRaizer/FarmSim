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
        private Canvas canvas;

        private ItemPositionManager attachedItem;
        private Image attachedItemImg;

        private void Awake()
        {
            canvas = FindObjectOfType<Canvas>();
            inventoryUI = GetComponent<InventoryUI>();
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
            // swap places
            attachedItem.SlotIndex = otherSlotIndex;
            inventoryUI.MoveImageToSlot(attachedItem.gameObject, otherSlotIndex);

            if (otherItem != null)
            {
                otherItem.SlotIndex = AttachedItemSlotIndex;
                inventoryUI.MoveImageToSlot(otherItem.gameObject, AttachedItemSlotIndex);
            }

            // make it clickable again
            attachedItemImg.raycastTarget = true;
            attachedItem = null;
            attachedItemImg = null;
        }

        public void SetAttachedItem(ItemPositionManager _attachedItem)
        {
            if (_attachedItem == null)
                return;

            attachedItem = _attachedItem;

            // set a new parent so it appears over all slots
            attachedItem.transform.SetParent(itemMovementParent);
            attachedItemImg = attachedItem.GetComponent<Image>();

            // make sure it isn't clickable
            attachedItemImg.raycastTarget = false;
        }

        public bool HasAttachedItem()
        {
            return attachedItem != null;
        }

        private void MoveAttachedImgToMouse()
        {
            if (attachedItemImg != null)
            {
                attachedItemImg.rectTransform.SetToMouse(canvas);
            }
        }
    }
}
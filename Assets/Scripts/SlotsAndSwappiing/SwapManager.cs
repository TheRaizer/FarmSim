using FarmSim.Utility;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace FarmSim.Slots
{
    /// <class name="SwapManager">
    ///     <summary>
    ///         Manages the movement of ISwappables from one slot to another.
    ///     </summary>
    /// </class>
    public class SwapManager : MonoBehaviour
    {
        [SerializeField] private Transform movementParent;

        private Canvas canvas;

        private ISwappable attachedSwappable;

        private void Awake()
        {
            canvas = FindObjectOfType<Canvas>();
        }

        private void Update()
        {
            MoveAttachedImgToMouse();
        }

        /// <summary>
        ///     Swaps the positions of 1-2 item <see cref="Image"/>'s in the inventory UI.
        ///     <para>
        ///         If there is an empty slot that can hold an item, other is null and a slotHandler must be given.
        ///     </para>
        /// </summary>
        /// <param name="otherSlotIndex">The slot index of the other item</param>
        /// <param name="other">The other item that will be swapped with the <see cref="attachedSwappable"/></param>
        /// <param name="slotHandler">Used to move the attached item to the correct empty slot</param>
        public void SwapPositions(int otherSlotIndex, ISwappable other, SlotsHandler slotHandler = null)
        {
            if (other != null)
            {
                if (attachedSwappable.AvoidSwap(other))
                    return;

                SlotsHandler attachedSlotHandler = attachedSwappable.SlotsHandler;

                // move other swappable to the attached swappables slot
                int temp_idx = attachedSwappable.SlotIndex;
                other.SlotIndex = attachedSwappable.SlotIndex;
                attachedSlotHandler.MoveImageToSlot(other.Icon.gameObject, temp_idx);

                // move attached swappable to the other swappables slot
                attachedSwappable.SlotIndex = otherSlotIndex;
                other.SlotsHandler.MoveImageToSlot(attachedSwappable.Icon.gameObject, otherSlotIndex);

                // swap which slot handler they will reference
                SlotsHandler temp_slots = attachedSlotHandler;
                attachedSwappable.SlotsHandler = other.SlotsHandler;
                other.SlotsHandler = temp_slots;
            }
            else
            {
                // move attached swappable to the empty slot
                attachedSwappable.SlotIndex = otherSlotIndex;
                slotHandler.MoveImageToSlot(attachedSwappable.Icon.gameObject, otherSlotIndex);


                // assign the slot handler of the empty slot to the attached swappable.
                attachedSwappable.SlotsHandler = slotHandler;
            }


            ResetAttachedSwappable();
        }

        public void SetAttachedSwappable(ISwappable _attachedSwappable)
        {
            attachedSwappable = _attachedSwappable;

            // set a new parent so it appears over all slots
            attachedSwappable.Icon.transform.SetParent(movementParent);

            // make sure it isn't clickable
            attachedSwappable.Icon.raycastTarget = false;
        }

        public ISwappable GetAttachedSwappable()
        {
            return attachedSwappable;
        }

        public void StopSwap()
        {
            // return the attached swappable back to its original slot
            attachedSwappable.SlotsHandler.MoveImageToSlot(attachedSwappable.Icon.gameObject, attachedSwappable.SlotIndex);
            ResetAttachedSwappable();
        }

        public void DestroyAttachedSwappable()
        {
            if (attachedSwappable != null)
            {
                attachedSwappable.OnDestroy();
                attachedSwappable = null;
            }
        }

        public bool HasAttachedSwappable()
        {
            return attachedSwappable != null;
        }

        private void MoveAttachedImgToMouse()
        {
            if (attachedSwappable != null)
            {
                attachedSwappable.Icon.rectTransform.SetToMouse(canvas);
            }
        }

        private void ResetAttachedSwappable()
        {
            // make it clickable again
            attachedSwappable.Icon.raycastTarget = true;
            attachedSwappable = null;
        }
    }
}
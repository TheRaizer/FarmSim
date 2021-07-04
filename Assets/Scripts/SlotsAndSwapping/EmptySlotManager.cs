using UnityEngine;
using UnityEngine.EventSystems;

namespace FarmSim.Slots
{
    /// <class name="EmptySlotManager">
    ///     <summary>
    ///         Manages the actions when a slot is empty.
    ///     </summary>
    /// </class>
    public class EmptySlotManager : MonoBehaviour, IPointerClickHandler
    {
        public int SlotIndex { private get; set; }
        /// <summary>
        ///     The slots handler that contains this slot. Assigned when the slot <see cref="GameObject"/> is instantiated.
        /// </summary>
        public SlotsHandler SlotsHandler { private get; set; }
        private SwapManager swapManager;

        private void Awake()
        {
            swapManager = FindObjectOfType<SwapManager>();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                if (swapManager.HasAttachedSwappable())
                {
                    // move attached item to this empty slot
                    swapManager.SwapPositions(SlotIndex, null, SlotsHandler);
                }
            }
        }
    }
}

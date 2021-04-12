using UnityEngine;
using UnityEngine.EventSystems;

namespace FarmSim.Slots
{
    /// <class name="EmptySlotManager">
    ///     <summary>
    ///         Manages the actions of an empty slot.
    ///     </summary>
    /// </class>
    public class EmptySlotManager : MonoBehaviour, IPointerClickHandler
    {
        public int SlotIndex { private get; set; }
        /// <summary>
        ///     The slots handler that contains this slot
        /// </summary>
        public SlotsHandler SlotsHandler { private get; set; }
        private SwapManager movementManager;

        private void Awake()
        {
            movementManager = FindObjectOfType<SwapManager>();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                if (movementManager.HasAttachedSwappable())
                {
                    // move attached item to this slot using the given index for the list of slots in the SlotsHandler
                    movementManager.SwapPositions(SlotIndex, null, SlotsHandler);
                }
            }
        }
    }
}

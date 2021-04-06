using FarmSim.Slots;
using UnityEngine;
using UnityEngine.EventSystems;

namespace FarmSim.Items
{
    /// <class name="SlotClickManager">
    ///     <summary>
    ///         Manages the actions that run when a slot is clicked.
    ///     </summary>
    /// </class>
    public class SlotClickManager : MonoBehaviour, IPointerClickHandler
    {
        public int SlotIndex { private get; set; }
        /// <summary>
        ///     The slots handler that contains this slot
        /// </summary>
        public InventorySlotsHandler SlotsHandler { private get; set; }
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

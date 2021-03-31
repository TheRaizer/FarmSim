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
        public ItemSlotsHandler SlotsHandler { private get; set; }
        private ItemMovementManager movementManager;

        private void Awake()
        {
            movementManager = FindObjectOfType<ItemMovementManager>();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                if (movementManager.HasAttachedItem())
                {
                    // move attached item to this slot using the given index for the list of slots in the SlotsHandler
                    movementManager.SwapPositions(SlotIndex, null, SlotsHandler);
                }
            }
        }
    }
}

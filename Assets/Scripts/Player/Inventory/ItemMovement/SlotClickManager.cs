using UnityEngine;
using UnityEngine.EventSystems;

namespace FarmSim.Player
{
    /// <class name="SlotClickManager">
    ///     <summary>
    ///         Manages the actions that run when a slot is clicked.
    ///     </summary>
    /// </class>
    public class SlotClickManager : MonoBehaviour, IPointerClickHandler
    {
        public int SlotIndex { private get; set; }

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
                    // if there is an attached item then swap it with this slot index
                    movementManager.SwapPositions(SlotIndex, null);
                }
            }
        }
    }
}

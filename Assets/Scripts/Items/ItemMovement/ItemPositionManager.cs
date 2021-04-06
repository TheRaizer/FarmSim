using FarmSim.Slots;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace FarmSim.Items
{
    /// <class name="ItemPositionManager">
    ///     <summary>
    ///         Manages the position of an Item Icon/<see cref="Image"/> in the inventory.
    ///     </summary>
    /// </class>
    public class ItemPositionManager : MonoBehaviour, IPointerClickHandler, IPositionManager
    {
        /// <summary>
        ///     The slots handler that contains the slot this item is in.
        /// </summary>
        public SlotsHandler SlotsHandler { get; set; }
        public ISwappable Swappable { get; private set; }

        private SwapManager movementManager;

        private void Awake()
        {
            movementManager = FindObjectOfType<SwapManager>();
        }

        public void SetSwappable(Item item) => Swappable = item;

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                if (movementManager.HasAttachedSwappable())
                {
                    movementManager.SwapPositions(Swappable.SlotIndex, this);
                }
                else
                {
                    movementManager.SetAttachedSwappable(Swappable);
                }
            }
        }
    }
}

using FarmSim.Utility;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace FarmSim.Items
{
    /// <class name="ItemPositionManager">
    ///     <summary>
    ///         Manages the position of an Item Icon/<see cref="Image"/> in the inventory.
    ///     </summary>
    /// </class>
    public class ItemPositionManager : MonoBehaviour, IPointerClickHandler
    {
        public Item Item { get; set; }
        /// <summary>
        ///     The slots handler that contains the slot this item is in.
        /// </summary>
        public InventorySlotsHandler SlotsHandler { get; set; }

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
                    movementManager.SwapPositions(Item.SlotIndex, this);
                }
                else
                {
                    // set this item as the attached item
                    movementManager.AttachedItemSlotIndex = Item.SlotIndex;
                    movementManager.SetAttachedItem(Item);
                }
            }
        }
    }
}

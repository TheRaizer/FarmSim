using FarmSim.Slots;
using UnityEngine;
using UnityEngine.EventSystems;

namespace FarmSim.Items
{
    /// <class name="ItemTrash">
    ///     <summary>
    ///         Deletes any item that is placed over it from the inventory.
    ///     </summary>
    /// </class>
    public class ItemTrash : MonoBehaviour, IPointerClickHandler
    {
        private SwapManager itemMovementManager;

        private void Awake()
        {
            itemMovementManager = FindObjectOfType<SwapManager>();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                itemMovementManager.DestroyAttachedSwappable();
            }
        }
    }
}

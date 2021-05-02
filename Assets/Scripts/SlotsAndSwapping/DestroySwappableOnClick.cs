using UnityEngine;
using UnityEngine.EventSystems;

namespace FarmSim.Slots
{
    /// <class name="DestroySwappableOnClick">
    ///     <summary>
    ///         Deletes the <see cref="SwapManager"/>'s attached <see cref="ISwappable"/> when clicked.
    ///     </summary>
    /// </class>
    public class DestroySwappableOnClick : MonoBehaviour, IPointerClickHandler
    {
        private SwapManager swapManager;

        private void Awake()
        {
            swapManager = FindObjectOfType<SwapManager>();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                swapManager.DestroyAttachedSwappable();
            }
        }
    }
}

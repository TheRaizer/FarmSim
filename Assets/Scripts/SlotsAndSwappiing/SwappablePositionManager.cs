using UnityEngine;
using UnityEngine.EventSystems;

namespace FarmSim.Slots
{
    /// <class name="SwappablePositionManager">
    ///     <summary>
    ///         Manages the position of an <see cref="ISwappable"/> using the <see cref="SwapManager"/>.
    ///     </summary>
    /// </class>
    public class SwappablePositionManager : MonoBehaviour, IPointerClickHandler
    {
        /// <summary>
        ///     Set when the items slot sprite is created. 
        /// </summary>
        public ISwappable Swappable { get; private set; }

        private SwapManager movementManager;

        private void Awake()
        {
            movementManager = FindObjectOfType<SwapManager>();
        }

        public void SetSwappable(ISwappable swappable) => Swappable = swappable;

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                if (movementManager.HasAttachedSwappable())
                {
                    // swap the positions of the attached swappable with this one.
                    movementManager.SwapPositions(Swappable.SlotIndex, Swappable);
                }
                else
                {
                    // set the attached swappable to be this.
                    movementManager.SetAttachedSwappable(Swappable);
                }
            }
        }
    }
}

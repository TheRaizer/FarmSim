using FarmSim.Items;
using FarmSim.Slots;
using UnityEngine.EventSystems;

namespace FarmSim.Shops
{
    /// <class name="ItemSell">
    ///     <summary>
    ///         When clicked opens the sell panel of given <see cref="Shop"/> with the <see cref="Item"/> held in the <see cref="SwapManager"/>.
    ///     </summary>
    /// </class>
    public class ItemSell : ShopReference, IPointerClickHandler
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
                OpenSellPanel();
            }
        }

        private void OpenSellPanel()
        {
            // get the swappable attached to the mouse
            ISwappable swappable = swapManager.GetAttachedSwappable();

            // if there is a swappable and its an item
            if (swappable != null && swappable.GetType() == typeof(Item))
            {
                // open the sell panel with that item.
                Item item = (Item)swappable;
                Shop.OpenSellPanel(item);

                // return the attached item back to its slot
                swapManager.StopSwap();
            }
        }
    }
}
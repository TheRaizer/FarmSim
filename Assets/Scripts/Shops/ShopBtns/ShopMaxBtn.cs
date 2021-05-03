using UnityEngine.EventSystems;

namespace FarmSim.Shops
{
    /// <class name="ShopMaxBtn">
    ///     <summary>
    ///         When clicked produces the max amount of items the player can sell or buy.
    ///     </summary>
    /// </class>
    public class ShopMaxBtn : ShopReference, IPointerUpHandler
    {
        public void OnPointerUp(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                Shop.BuySellMax();
            }
        }
    }
}
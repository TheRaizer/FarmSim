using UnityEngine.EventSystems;

namespace FarmSim.Shops
{
    /// /// <class name="ShopDecisionBtn">
    ///     <summary>
    ///         Placed on a button to make a decision for the current opened shop when pressed.
    ///     </summary>
    /// </class>
    public class ShopDecisionBtn : ShopReference, IPointerUpHandler
    {
        public void OnPointerUp(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                Shop.MakeDecision();
            }
        }
    }
}

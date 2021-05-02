using UnityEngine.EventSystems;

namespace FarmSim.Shops 
{
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

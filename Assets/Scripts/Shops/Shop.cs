using FarmSim.Entity;
using FarmSim.Items;
using UnityEngine;

namespace FarmSim.Shops
{
    /// <class name="Shop">
    ///     <summary>
    ///         Component attached to any shop which manages buying and selling.
    ///     </summary>
    /// </class>
    public class Shop : MonoBehaviour
    {
        [SerializeField] private GameObject buyPanel;
        [SerializeField] private CurrencyManager playerCurrencyManager;

        private bool canBuy = false;

        private void Awake()
        {
            var shopSlots = GetComponent<ShopSlotsHandler>();
            shopSlots.AssignShopIconDelegate = AssignShopIconOpenPanel;
        }

        public void OpenBuyPanel(int cost, string name)
        {
            
        }

        public void Buy(int cost)
        {

        }

        public void Sell(ItemType itemType)
        {

        }

        private void AssignShopIconOpenPanel(GameObject icon)
        {
            var shopIcon = icon.GetComponent<ShopIcon>();
            shopIcon.OpenBuyPanel = OpenBuyPanel;
        }
    }
}

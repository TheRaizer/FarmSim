using FarmSim.Entity;
using FarmSim.Items;
using FarmSim.Utility;
using System.Collections.Generic;
using TMPro;
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
        [Header("Main UI")]
        [SerializeField] private GameObject shopUI;
        [SerializeField] private OnPress openButton;

        [Header("Buy Panel")]
        [SerializeField] private GameObject buyPanel;
        [SerializeField] private TextMeshProUGUI itemNameText;
        [SerializeField] private TextMeshProUGUI priceText;
        [SerializeField] private TextMeshProUGUI amtText;

        [Header("Player Currency")]
        [SerializeField] private CurrencyManager playerCurrencyManager;

        [Header("Shop Config")]
        [SerializeField] private List<ItemType> buyables;
        [SerializeField] private string shopId;

        private bool CanBuy => TotalCost <= playerCurrencyManager.CurrentAmt;
        private int TotalCost => itemToBuy.Price * amtToBuy;

        private int amtToBuy = 1;
        private ItemType itemToBuy;

        /// <summary>
        ///     A <see cref="Shop"/> is connected to a <see cref="ShopSlotsHandler"/>/UI when it is on the same GameObject.
        ///     
        ///     <para>
        ///         Multiple <see cref="Shop"/>'s can use the same <see cref="ShopSlotsHandler"/>.
        ///     </para>
        ///     <para>
        ///         You may use a different style UI for a different shop by pairing a <see cref="Shop"/> and <see cref="ShopSlotsHandler"/>
        ///         together on a new GameObject in the canvas.
        ///     </para>
        /// </summary>
        private ShopSlotsHandler shopSlots;

        private void Awake()
        {
            openButton.actions.Add(InvertShopActiveness);
        }

        private void Start()
        {
            var shopSlots = GetComponent<ShopSlotsHandler>();
            shopSlots.OnIconCreation = AssignShopIconOpenPanel;
            shopSlots.AddShopSpritesToSlot(buyables, shopId);
            this.shopSlots = shopSlots;
        }

        public void InvertShopActiveness()
        {
            if (shopUI.activeInHierarchy)
            {
                shopUI.SetActive(false);
                return;
            }

            shopUI.SetActive(true);
            shopSlots.ActivateShopSprites(shopId);
        }

        public void OpenBuyPanel(ItemType itemType)
        {
            itemToBuy = itemType;
            amtToBuy = 1;
            itemNameText.SetText(itemType.ItemName);
            buyPanel.SetActive(true);
        }

        public void Buy()
        {
            if (CanBuy)
            {
                playerCurrencyManager.DecreaseAmt(amtToBuy * itemToBuy.Price);
            }
        }

        public void Sell(ItemType itemType)
        {

        }

        private void AssignShopIconOpenPanel(GameObject icon)
        {
            var shopIcon = icon.GetComponent<ShopIcon>();
            shopIcon.OpenBuyPanel = OpenBuyPanel;
        }
        
        private void SetTexts()
        {
            priceText.SetText("Price: " + TotalCost);
            amtText.SetText("Amount: " + amtToBuy);
        }

        public void IncrementAmtToBuy()
        {
            amtToBuy++;

            SetTexts();

            if (CanBuy)
            {
                priceText.color = Color.red;
                amtText.color = Color.red;
            }
        }

        public void DecrementAmtToBuy()
        {
            if (amtToBuy <= 1)
                return;

            amtToBuy--;

            SetTexts();

            if (!CanBuy)
            {
                priceText.color = Color.black;
                amtText.color = Color.black;
            }
        }
    }
}

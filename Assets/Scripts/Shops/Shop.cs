using FarmSim.Entity;
using FarmSim.Items;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FarmSim.Shops
{
    /// <class name="Shop">
    ///     <summary>
    ///         Component attached to any shop which manages buying and selling of items.
    ///     </summary>
    /// </class>
    public class Shop : MonoBehaviour
    {
        private const float SELL_DECR = 0.65f;

        [Header("Main UI")]
        [SerializeField] private GameObject shopUI;

        [Header("Exchange Panel")]
        [SerializeField] private GameObject exchangePanel;
        [SerializeField] private TextMeshProUGUI itemNameTxt;
        [SerializeField] private TextMeshProUGUI moneyTxt;
        [SerializeField] private TextMeshProUGUI amtTxt;
        [SerializeField] private TextMeshProUGUI decisionBtnTxt;
        [SerializeField] private Button decisionBtn;

        [Header("Player Currency")]
        [SerializeField] private CurrencyManager playerCurrencyManager;

        [Header("Shop Config")]
        [SerializeField] private List<ItemType> buyables;
        [SerializeField] private string shopId;

        private bool CanBuy => (TotalCost <= playerCurrencyManager.CurrentAmt) && (!inventory.WillOverFlowOnAdd(itemToBuy, amtToExchange));
        private int TotalCost => itemToBuy.Price * amtToExchange;
        private int SellValue => Mathf.FloorToInt(itemToSell.itemType.Price * amtToExchange * SELL_DECR);

        private int amtToExchange = 0;
        private bool isBuying = false;
        private ItemType itemToBuy;
        private Item itemToSell;

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
        private Inventory inventory;

        private void Awake()
        {
            inventory = FindObjectOfType<Inventory>();
        }

        private void Start()
        {
            var shopSlots = GetComponent<ShopSlotsHandler>();
            shopSlots.OnIconCreation = AssignShopIconOpenPanel;
            shopSlots.AddShopSpritesToSlot(buyables, shopId);
            this.shopSlots = shopSlots;
        }

        // Unity btn event
        public void InvertShopActiveness()
        {
            if (shopUI.activeInHierarchy)
            {
                shopUI.SetActive(false);
                Time.timeScale = 1;
                return;
            }

            Time.timeScale = 0;
            shopUI.SetActive(true);
            shopSlots.ActivateShopSprites(shopId);
        }

        // Unity btn event
        public void MakeDecision()
        {
            if (isBuying)
            {
                Buy();
            }
            else
            {
                Sell();
            }

            CloseExchangePanel();
        }

        public void IncrementAmt()
        {
            // if they try to sell more then they have.
            if (!isBuying && amtToExchange >= itemToSell.Amt)
                return;

            amtToExchange++;

            DetermineInteractabilityOnBuy();

            SetTexts();
        }

        public void DecrementAmt()
        {
            if (amtToExchange <= 1)
                return;


            amtToExchange--;

            DetermineInteractabilityOnBuy();

            SetTexts();
        }

        // Unity btn event
        public void CloseExchangePanel()
        {
            exchangePanel.SetActive(false);
            shopUI.SetActive(true);
        }

        public void OpenSellPanel(Item item)
        {
            // reset UI
            ResetExchangeUI();
            shopUI.SetActive(false);

            // assign variables
            isBuying = false;
            itemToSell = item;
            amtToExchange = 0;

            IncrementAmt();

            // set texts for the exchange panel
            decisionBtnTxt.SetText("Sell");
            itemNameTxt.SetText(item.itemType.ItemName);
            SetTexts();

            // show the exchange panel
            exchangePanel.SetActive(true);
        }

        public void OpenBuyPanel(ItemType itemType)
        {
            ResetExchangeUI();
            shopUI.SetActive(false);

            // assign variables
            isBuying = true;
            itemToBuy = itemType;
            amtToExchange = 0;

            IncrementAmt();

            // set texts for the exchange panel
            decisionBtnTxt.SetText("Buy");
            itemNameTxt.SetText(itemType.ItemName);
            SetTexts();

            // show the exchange panel
            exchangePanel.SetActive(true);
        }

        private void DetermineInteractabilityOnBuy()
        {
            if (isBuying)
            {
                if (!CanBuy)
                {

                    moneyTxt.color = Color.red;
                    amtTxt.color = Color.red;

                    decisionBtn.interactable = false;
                }
                else if (!decisionBtn.interactable && CanBuy)
                {

                    ResetExchangeUI();
                }
            }
        }

        private void ResetExchangeUI()
        {
            moneyTxt.color = Color.black;
            amtTxt.color = Color.black;

            decisionBtn.interactable = true;
        }

        private void AssignShopIconOpenPanel(GameObject icon)
        {
            var shopIcon = icon.GetComponent<ShopIcon>();
            shopIcon.OpenBuyPanel = OpenBuyPanel;
        }

        private void SetTexts()
        {
            moneyTxt.SetText(isBuying ? "Price: " + TotalCost : "Value: " + SellValue);
            amtTxt.SetText("Amount: " + amtToExchange);
        }

        private void Buy()
        {
            playerCurrencyManager.DecreaseAmt(TotalCost);
            inventory.AddToInventory(itemToBuy, amtToExchange);
        }

        private void Sell()
        {
            playerCurrencyManager.IncreaseAmt(SellValue);
            inventory.TakeFromInventory(itemToSell.guid, amtToExchange);
        }
    }
}

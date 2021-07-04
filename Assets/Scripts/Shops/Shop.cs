using FarmSim.Items;
using FarmSim.Player;
using System.Collections.Generic;
using UnityEngine;

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

        /// <summary>
        ///     A <see cref="Shop"/> is connected to a <see cref="ShopSlotsHandler"/>/UI when it is on the same GameObject.
        ///     
        ///     <para>
        ///         Multiple <see cref="Shop"/>'s can use the same <see cref="ShopSlotsHandler"/>.
        ///     </para>
        ///     <para>
        ///         You may use a different style UI for a different shop by pairing a <see cref="Shop"/> and different <see cref="ShopSlotsHandler"/>'s
        ///         together.
        ///     </para>
        /// </summary>
        [SerializeField] private ShopSlotsHandler shopSlots;
        [SerializeField] private ShopUIHandler shopUIHandler;
        [SerializeField] private GameObject shopParent;

        [Header("Shop Config")]
        [SerializeField] private List<ItemType> buyables;
        [SerializeField] private string shopId;

        private bool CanBuy => (itemToBuy != null) && (TotalCost <= playerCurrency.CurrentAmt) && (!inventory.WillOverFlowOnAdd(itemToBuy, amtToExchange));
        private int TotalCost => itemToBuy == null ? 0 : itemToBuy.Price * amtToExchange;
        private int SellValue => itemToSell == null ? 0 : Mathf.FloorToInt(itemToSell.itemType.Price * SELL_DECR) * amtToExchange;

        private int amtToExchange = 0;
        private bool isBuying = false;

        private ItemType itemToBuy;
        private Item itemToSell;
        private PlayerCurrency playerCurrency;

        /// <summary>
        ///     Holds any components who are dependent on knowing what shop is currently opened.
        /// </summary>
        private readonly List<ShopReference> references = new List<ShopReference>();
        private Inventory inventory;

        private void Awake()
        {
            inventory = FindObjectOfType<Inventory>();
            playerCurrency = FindObjectOfType<PlayerCurrency>();
            AssignShopReferences(shopParent.transform);
        }

        private void Start()
        {
            shopSlots.OnIconCreation = AssignShopIconOpenPanel;
            shopSlots.AddShopSpritesToSlot(buyables, shopId);
        }

        /// <summary>
        ///     Adds to the list of components that will need to reference this shop once it is opened.
        /// </summary>
        /// <param name="parent">The parent of any child <see cref="GameObject"/>'s</param>
        private void AssignShopReferences(Transform parent)
        {
            // iterate through each child
            foreach (Transform child in parent)
            {
                // if there is a shop reference in any GameObjects
                if (child.TryGetComponent(out ShopReference reference))
                {
                    // add that to the list of references
                    references.Add(reference);
                }

                if (child.childCount > 0)
                {
                    // if the child has children recurse
                    AssignShopReferences(child);
                }
            }
        }

        public void IncrementAmt()
        {
            // if they try to sell more then they have.
            if (!isBuying && amtToExchange >= itemToSell.Amt)
                return;

            amtToExchange++;

            shopUIHandler.DetermineInteractabilityOnBuy(isBuying, CanBuy);
            shopUIHandler.SetTexts(isBuying, TotalCost, SellValue, amtToExchange);
        }

        public void DecrementAmt()
        {
            if (amtToExchange <= 1)
                return;

            amtToExchange--;

            shopUIHandler.DetermineInteractabilityOnBuy(isBuying, CanBuy);
            shopUIHandler.SetTexts(isBuying, TotalCost, SellValue, amtToExchange);
        }

        public void OpenSellPanel(Item item)
        {
            shopUIHandler.ResetExchangeUI();

            // assign variables
            isBuying = false;
            itemToSell = item;
            amtToExchange = 0;

            IncrementAmt();

            shopUIHandler.OpenExchangePanel(item.itemType.ItemName, isBuying, TotalCost, SellValue, amtToExchange, "Sell");
        }

        public void OpenBuyPanel(ItemType itemType)
        {
            shopUIHandler.ResetExchangeUI();

            // assign variables
            isBuying = true;
            itemToBuy = itemType;
            amtToExchange = 0;

            IncrementAmt();

            shopUIHandler.OpenExchangePanel(itemType.ItemName, isBuying, TotalCost, SellValue, amtToExchange, "Buy");
        }

        private void AssignShopIconOpenPanel(GameObject icon)
        {
            var shopIcon = icon.GetComponent<ShopIcon>();
            shopIcon.OpenBuyPanel = OpenBuyPanel;
        }

        private void Buy()
        {
            playerCurrency.DecreaseAmt(TotalCost);
            inventory.AddToInventory(itemToBuy, amtToExchange);
        }

        private void Sell()
        {
            playerCurrency.IncreaseAmt(SellValue);
            inventory.TakeFromInventory(itemToSell.guid, amtToExchange);
        }

        /// <summary>
        /// Unity btn event
        /// </summary>
        public void InvertShopActiveness()
        {
            bool opened = shopUIHandler.InvertShopActiveness();
            if (opened)
            {
                // if the shop was opened assign this shop to any references and activate the sprites.
                references.ForEach(x => x.Shop = this);
                shopSlots.ActivateShopSprites(shopId);
            }
        }

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

            shopUIHandler.CloseExchangePanel();
        }

        public void BuySellMax()
        {
            if (isBuying)
                amtToExchange = Mathf.FloorToInt((float)playerCurrency.CurrentAmt / itemToBuy.Price);
            else
                amtToExchange = itemToSell.Amt;

            shopUIHandler.SetTexts(isBuying, TotalCost, SellValue, amtToExchange);
        }
    }
}

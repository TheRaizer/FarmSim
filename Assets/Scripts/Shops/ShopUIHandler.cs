using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FarmSim.Shops
{
    /// <class name="ShopUIHandler">
    ///     <summary>
    ///         Manages the UI of a shop.
    ///     </summary>
    /// </class>
    public class ShopUIHandler : MonoBehaviour
    {
        [SerializeField] private GameObject shopSlotsUI;

        [Header("Exchange Panel")]
        [SerializeField] private GameObject exchangePanel;
        [SerializeField] private TextMeshProUGUI itemNameTxt;
        [SerializeField] private TextMeshProUGUI moneyTxt;
        [SerializeField] private TextMeshProUGUI amtTxt;
        [SerializeField] private TextMeshProUGUI decisionBtnTxt;
        [SerializeField] private Button decisionBtn;

        public void OpenExchangePanel(string itemName, bool isBuying, int TotalCost, int SellValue, int amtToExchange, string text)
        {
            shopSlotsUI.SetActive(false);

            // set texts for the exchange panel
            decisionBtnTxt.SetText(text);
            itemNameTxt.SetText(itemName);
            SetTexts(isBuying, TotalCost, SellValue, amtToExchange);

            // show the exchange panel
            exchangePanel.SetActive(true);
        }

        public void SetTexts(bool isBuying, int TotalCost, int SellValue, int amtToExchange)
        {
            moneyTxt.SetText(isBuying ? "Price: " + TotalCost : "Value: " + SellValue);
            amtTxt.SetText("Amount: " + amtToExchange);
        }

        public void CloseExchangePanel()
        {
            exchangePanel.SetActive(false);
            shopSlotsUI.SetActive(true);
        }
        public void ResetExchangeUI()
        {
            moneyTxt.color = Color.black;
            amtTxt.color = Color.black;

            decisionBtn.interactable = true;
        }

        public void DetermineInteractabilityOnBuy(bool isBuying, bool CanBuy)
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

        /// <summary>
        ///     Changes the activeness of a shops UI.
        /// </summary>
        /// <returns>Whether the shop was opened or closed.</returns>
        public bool InvertShopActiveness()
        {
            if (shopSlotsUI.activeInHierarchy)
            {
                shopSlotsUI.SetActive(false);
                Time.timeScale = 1;
                return false;
            }

            Time.timeScale = 0;
            shopSlotsUI.SetActive(true);
            return true;
        }
    }
}
using FarmSim.Items;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace FarmSim.Shops
{
    /// <class name="ShopIcon">
    ///     <summary>
    ///         Component attached to any shop icon that opens the buy panel when clicked.
    ///     </summary>
    /// </class>
    public class ShopIcon : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private ItemType itemType;

        public Action<ItemType> OpenBuyPanel { private get; set; }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                OpenBuyPanel(itemType);
            }
        }
    }
}
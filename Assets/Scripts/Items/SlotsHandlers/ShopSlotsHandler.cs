using System.Collections.Generic;
using UnityEngine;
using FarmSim.Utility;

namespace FarmSim.Items
{
    public class ShopSlotsHandler : SlotsHandler
    {
        [SerializeField] private List<ItemType> buyables;

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void ManageSlotOnLoad(GameObject slot, int slotIndex)
        {
            GameObject shopSprite = Instantiate(buyables[slotIndex].ShopIconPrefab, slot.transform);
            var rect = shopSprite.GetComponent<RectTransform>();

            // reset its position to 0
            rect.SetLeft(0);
            rect.SetRight(0);
            rect.SetTop(0);
            rect.SetBottom(0);

            // reset its scale to 1
            rect.localScale = Vector3.one;
        }
    }
}

using System.Collections.Generic;
using UnityEngine;
using FarmSim.Utility;

namespace FarmSim.Items
{
    /// <class name="ShopSlotsHandler">
    ///     <summary>
    ///         Manages the slots of a given content panel relating to a shops UI.
    ///     </summary>
    /// </class>
    public class ShopSlotsHandler : SlotsHandler
    {
        [SerializeField] private List<ItemType> buyables;

        protected override void Awake()
        {
            base.Awake();
            if (buyables.Count > slots.Count)
                Debug.LogError("Cannot have more buyables than slots");
        }

        protected override void ManageSlotOnLoad(GameObject slot, int slotIndex)
        {
            GameObject shopSprite = Instantiate(buyables[slotIndex].ShopIconPrefab, slot.transform);
            var rect = shopSprite.GetComponent<RectTransform>();

            rect.Center();

            // reset its scale to 1
            rect.localScale = Vector3.one;
        }
    }
}

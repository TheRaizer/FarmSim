using FarmSim.Items;
using FarmSim.Slots;
using FarmSim.Utility;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FarmSim.Shops
{
    /// <class name="ShopSlotsHandler">
    ///     <summary>
    ///         Manages the slots of a given content panel relating to a shops UI.
    ///     </summary>
    /// </class>
    public class ShopSlotsHandler : SlotsHandler
    {
        public Action<GameObject> OnIconCreation { private get; set; }

        private readonly Dictionary<string, List<GameObject>> shopImages = new Dictionary<string, List<GameObject>>();

        private string currentShopId = "";

        /// <summary>
        ///     Adds Shop sprites to the slots. (Must be run after Awake. Due to unloaded slots.)
        /// </summary>
        /// <param name="buyables">The list of items whose images must be added to the slots.</param>
        /// <param name="shopId">Used to identify which images to activate/deactivate.</param>
        public void AddShopSpritesToSlot(List<ItemType> buyables, string shopId)
        {
            if(buyables.Count >= slots.Count)
            {
                Debug.LogError("Too many buyables for the number of existing slots");
                return;
            }

            List<GameObject> itemImages = new List<GameObject>();

            for(int i = 0; i < buyables.Count; i++)
            {
                Image slot = slots[i];
                GameObject itemImage = Instantiate(buyables[i].ShopIconPrefab, slot.transform);

                OnIconCreation(itemImage);

                itemImage.SetActive(false);
                var rect = itemImage.GetComponent<RectTransform>();

                rect.Center();

                // reset its scale to 1
                rect.localScale = Vector3.one;

                itemImages.Add(itemImage);
            }

            shopImages.Add(shopId, itemImages);
        }

        public void ActivateShopSprites(string shopId)
        {
            if (!shopImages.ContainsKey(shopId))
                Debug.LogError($"There are no such shop sprites with id {shopId}");

            DeactivateCurrentShopImages();

            currentShopId = shopId;

            foreach (GameObject g in shopImages[currentShopId])
            {
                g.SetActive(true);
            }
        }

        private void DeactivateCurrentShopImages()
        {
            if (shopImages.TryGetValue(currentShopId, out List<GameObject> objectsToDeactivate))
            {
                foreach (GameObject g in objectsToDeactivate)
                {
                    g.SetActive(false);
                }
            }
        }
    }
}

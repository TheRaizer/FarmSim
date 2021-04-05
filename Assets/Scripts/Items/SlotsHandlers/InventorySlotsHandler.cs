using UnityEngine;
using UnityEngine.UI;
using FarmSim.Utility;

namespace FarmSim.Items
{
    /// <class name="InventorySlotsHandler">
    ///     <summary>
    ///         Manages the slots of a given content panel in the UI.
    ///     </summary>
    /// </class>
    public class InventorySlotsHandler : SlotsHandler
    {
        public void AddImageToSlot(Item item)
        {
            if (slotPrefab == null && contentParent == null)
                return;
            
            for (int i = 0; i < slots.Count; i++)
            {
                if (slots[i].transform.childCount <= 0)
                {
                    Image slotImg = slots[i];

                    SpawnImage(item, slotImg, i);
                    return;
                }
            }
        }

        public void MoveImageToSlot(GameObject obj, int slotIndex)
        {
            Image slotImg = slots[slotIndex];
            Image image = obj.GetComponent<Image>();

            obj.transform.SetParent(slotImg.transform);

            // sets the image center to the slot center
            image.rectTransform.anchoredPosition = Vector3.zero;
        }

        protected override void ManageSlotOnLoad(GameObject slot, int slotIndex)
        {
            // assign click manager properties
            var slotManager = slot.GetComponent<SlotClickManager>();
            slotManager.SlotIndex = slotIndex;
            slotManager.SlotsHandler = this;
        }

        protected void SpawnImage(Item item, Image slotImg, int slotIndex)
        {
            GameObject itemObj = item.SpawnImageObject(slotIndex, this);
            itemObj.transform.SetParent(slotImg.transform);
            var rect = itemObj.GetComponent<RectTransform>();

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

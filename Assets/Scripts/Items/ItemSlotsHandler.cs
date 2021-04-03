using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FarmSim.Utility;

namespace FarmSim.Items
{
    /// <class name="ItemSlotsHandler">
    ///     <summary>
    ///         Manages the slots of a given content panel in the UI.
    ///     </summary>
    /// </class>
    public class ItemSlotsHandler : MonoBehaviour
    {
        [SerializeField] private bool loadOnAwake = false;

        [SerializeField] private int numberOfSlots;
        [SerializeField] protected GameObject slotPrefab;
        [SerializeField] protected GameObject contentParent;

        // first slot is the top left slot
        [SerializeField] private float firstSlotX = -87;
        [SerializeField] private float firstSlotY = 331;

        [field: SerializeField] public int SlotsInRow { get; private set; } = 4;

        protected readonly List<Image> slots = new List<Image>();

        private const int OFFSET = 60;

        protected virtual void Awake()
        {
            if (loadOnAwake)
                LoadSlots(numberOfSlots);
        }

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

        public void LoadSlots(int numOfSlots)
        {
            int y = 0;
            int slotIndex = 0;

            while (true)
            {
                for (int x = 0; x < SlotsInRow; x++)
                {
                    GameObject slot = Instantiate(slotPrefab, contentParent.transform);

                    // assign click manager properties
                    var slotManager = slot.GetComponent<SlotClickManager>();
                    slotManager.SlotIndex = slotIndex;
                    slotManager.SlotsHandler = this;

                    Image slotImg = slot.GetComponent<Image>();

                    slotImg.rectTransform.anchoredPosition = new Vector3(x * OFFSET + firstSlotX, y * -OFFSET + firstSlotY);


                    slots.Add(slotImg);

                    slotIndex++;

                    if (slotIndex >= numOfSlots)
                    {
                        return;
                    }
                }
                y++;
            }
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

            // center the image in the slot
            item.Icon.rectTransform.anchoredPosition = Vector3.zero;
        }
    }
}

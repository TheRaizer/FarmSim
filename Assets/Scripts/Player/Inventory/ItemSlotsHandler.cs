using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FarmSim.Player
{
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
            {
                return;
            }
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

                    // assign the slot index to the click manager
                    slot.GetComponent<SlotClickManager>().SlotIndex = slotIndex;

                    Image slotImg = slot.GetComponent<Image>();

                    // initialize slotImg scale and position
                    slotImg.rectTransform.localScale = new Vector3(2.0197f, 2.0197f, 2.0197f);
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
            GameObject itemObj = item.SpawnImageObject(slotIndex);
            itemObj.transform.SetParent(slotImg.transform);

            // REMOVE THIS LATER. CREATE INVENTORY SPECIFIC SPRITES
            item.Icon.rectTransform.localScale = new Vector3(0.2425136f, 0.2425136f, 0.2425136f);
            item.Icon.rectTransform.anchoredPosition = Vector3.zero;
        }
    }
}

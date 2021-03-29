using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

namespace FarmSim.Player 
{
    /// <class name="InventoryUI">
    ///     <summary>
    ///         Controls the slots in the inventory UI as well as the <see cref="Image"/>'s within the slots.
    ///     </summary>
    /// </class>
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField] private GameObject slotPrefab;
        [SerializeField] private GameObject contentParent;

        public const int SLOTS_IN_ROW = 4;

        private readonly List<Image> slots = new List<Image>();

        // first slot is the top left slot
        private const int FIRST_SLOT_X = -87;
        private const int FIRST_SLOT_Y = 331;

        private const int OFFSET = 60;

        public void AddImageToSlot(Item item)
        {
            if (slotPrefab == null && contentParent == null)
            {
                return;
            }
            for(int i = 0; i < slots.Count; i++)
            {
                if(slots[i].transform.childCount <= 0)
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

        public void LoadSlots(int numOfSlots, List<Item> inventory)
        {
            int y = 0;
            int slotIndex = 0;

            while (true)
            {
                for (int x = 0; x < SLOTS_IN_ROW; x++)
                {
                    GameObject slot = Instantiate(slotPrefab);

                    // assign the slot index to the click manager
                    slot.GetComponent<SlotClickManager>().SlotIndex = slotIndex;

                    Image slotImg = slot.GetComponent<Image>();

                    slotImg.transform.SetParent(contentParent.transform);
                    // initialize slotImg scale and position
                    slotImg.rectTransform.localScale = new Vector3(2.0197f, 2.0197f, 2.0197f);
                    slotImg.rectTransform.anchoredPosition = new Vector2(x * OFFSET + FIRST_SLOT_X, y * -OFFSET + FIRST_SLOT_Y);


                    slots.Add(slotImg);

                    slotIndex++;

                    if (slotIndex >= numOfSlots)
                    {
                        LoadItemImagesIntoSlots(inventory);
                        return;
                    }
                }
                y++;
            }
        }

        private void LoadItemImagesIntoSlots(List<Item> inventory)
        {
            inventory.ForEach(item =>
            {
                Image slotImg = slots[item.SlotIndex].GetComponent<Image>();
                SpawnImage(item, slotImg, item.SlotIndex);
            });
        }

        private void SpawnImage(Item item, Image slotImg, int slotIndex)
        {
            GameObject itemObj = item.SpawnImageObject(slotIndex);
            itemObj.transform.SetParent(slotImg.transform);

            // REMOVE THIS LATER. CREATE INVENTORY SPECIFIC SPRITES
            item.Icon.rectTransform.localScale = new Vector3(0.2425136f, 0.2425136f, 0.2425136f);
            item.Icon.rectTransform.anchoredPosition = Vector3.zero;
        }
    }
}
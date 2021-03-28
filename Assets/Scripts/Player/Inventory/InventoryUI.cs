using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using FarmSim.Utility;

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

        public void AddImageToSlot(Item item, int slotIndex)
        {
            if (slotPrefab != null && contentParent != null)
            {
                Image slotImg = slots[slotIndex];
                SpawnImage(item, slotImg, slotIndex);
            }
        }

        public void MoveImageToSlot(GameObject obj, int slotIndex)
        {
            Image slotImg = slots[slotIndex];
            Image image = obj.GetComponent<Image>();

            obj.transform.SetParent(slotImg.transform);

            image.rectTransform.anchoredPosition = Vector3.zero;
        }

        public void InitializeSlots(int numOfSlots, List<Item> inventory)
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

                    // if the inventory has an item that can be slotted
                    if (slotIndex < inventory.Count)
                    {
                        // get the item and spawn an image at the slot
                        Item item = inventory[slotIndex];
                        SpawnImage(item, slotImg, slotIndex);
                    }

                    slots.Add(slotImg);

                    slotIndex++;

                    if (slotIndex >= numOfSlots)
                        return;
                }
                y++;
            }
        }

        private void SpawnImage(Item item, Image slotImg, int slotIndex)
        {
            GameObject itemObj = Instantiate(item.itemType.IconPrefab);

            if(itemObj.TryGetComponent(out IReferenceGUID guid))
            {
                guid.Guid = item.guid;
            }

            // assign the slot index to the position manager for movement of items
            itemObj.GetComponent<ItemPositionManager>().SlotIndex = slotIndex;

            Image image = itemObj.GetComponent<Image>();

            // assign the placeable spawner to the item
            item.Icon = image;
            itemObj.transform.SetParent(slotImg.transform);

            // REMOVE THIS LATER. CREATE INVENTORY SPECIFIC SPRITES
            image.rectTransform.localScale = new Vector3(0.2425136f, 0.2425136f, 0.2425136f);
            image.rectTransform.anchoredPosition = Vector3.zero;
        }
    }
}
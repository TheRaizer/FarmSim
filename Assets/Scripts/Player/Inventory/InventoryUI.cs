using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

namespace FarmSim.Player 
{

    public class InventoryUI : MonoBehaviour
    {
        [SerializeField] private GameObject slotPrefab;
        [SerializeField] private GameObject contentParent;

        public List<Image> Slots { get; private set; } = new List<Image>();

        // first slot is the top left slot
        private const int FIRST_SLOT_X = -87;
        private const int FIRST_SLOT_Y = 331;

        private const int OFFSET = 60;

        public void InitializeSlots(int numOfSlots, List<Item> inventory)
        {
            int y = 0;
            int slotsAdded = 0;

            while (true)
            {
                for (int x = 0; x < 4; x++)
                {
                    slotsAdded++;

                    if(slotsAdded > numOfSlots)
                        return;

                    GameObject slot = Instantiate(slotPrefab);
                    Image img = slot.GetComponent<Image>();

                    img.transform.SetParent(contentParent.transform);

                    img.rectTransform.localScale = new Vector3(2.0197f, 2.0197f, 2.0197f);
                    img.rectTransform.anchoredPosition = new Vector2(x * OFFSET + FIRST_SLOT_X, y * -OFFSET + FIRST_SLOT_Y);
                    // spawn an item from the inventory HERE

                    Slots.Add(img);
                }
                y++;
            }
        }
    }
}
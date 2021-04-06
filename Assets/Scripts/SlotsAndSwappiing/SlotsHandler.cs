using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FarmSim.Slots
{
    /// <class name="SlotsHandler">
    ///     <summary>
    ///         Base class for managing a list of slots.
    ///     </summary>
    /// </class>
    public class SlotsHandler : MonoBehaviour
    {
        [SerializeField] protected bool loadOnAwake = false;
        [SerializeField] protected int numberOfSlots;

        [SerializeField] protected GameObject slotPrefab;
        [SerializeField] protected GameObject contentParent;

        // first slot is the top left slot
        [SerializeField] private Vector2 firstSlotPos = new Vector2(-87, 331);

        [field: SerializeField] public int SlotsInRow { get; private set; } = 4;

        protected readonly List<Image> slots = new List<Image>();

        private const int OFFSET = 60;

        protected virtual void Awake()
        {
            if (loadOnAwake)
                LoadSlots(numberOfSlots);
        }

        public void MoveImageToSlot(GameObject obj, int slotIndex)
        {
            Image slotImg = slots[slotIndex];
            Image image = obj.GetComponent<Image>();

            obj.transform.SetParent(slotImg.transform);

            // sets the image center to the slot center
            image.rectTransform.anchoredPosition = Vector3.zero;
        }

        protected virtual void ManageSlotOnLoad(GameObject slot, int slotIndex) { }

        /// <summary>
        ///     Loads slot images into their corrosponding positions in a given content panel.
        /// </summary>
        /// <param name="numOfSlots">The number of slots to Instantiate.</param>
        public void LoadSlots(int numOfSlots)
        {
            int y = 0;
            int slotIndex = 0;

            while (true)
            {
                for (int x = 0; x < SlotsInRow; x++)
                {
                    GameObject slot = Instantiate(slotPrefab, contentParent.transform);

                    ManageSlotOnLoad(slot, slotIndex);

                    Image slotImg = slot.GetComponent<Image>();

                    slotImg.rectTransform.anchoredPosition = new Vector3(x * OFFSET + firstSlotPos.x, y * -OFFSET + firstSlotPos.y);


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
    }
}
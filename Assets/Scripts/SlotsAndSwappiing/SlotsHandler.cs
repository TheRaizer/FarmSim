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

        [SerializeField] private int offset = 60;

        [field: SerializeField] public int SlotsInRow { get; private set; } = 4;

        protected readonly List<Image> slots = new List<Image>();

        protected virtual void Awake()
        {
            if (loadOnAwake)
                AddSlots(numberOfSlots);
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
        ///     Adds slot images into their corrosponding positions in a given content panel.
        /// </summary>
        /// <param name="numOfSlots">The number of slots to Instantiate.</param>
        public void AddSlots(int numOfSlots)
        {
            // calculate the starting y/row 0-based
            int y = slots.Count == 0 ? 0 : Mathf.FloorToInt((float)slots.Count / SlotsInRow);

            // find the slot index to start adding from
            int slotIndex = Mathf.Clamp(slots.Count, 0, int.MaxValue);

            // get the starting x
            int startX = slots.Count - (y * SlotsInRow);
            Debug.Log(startX);

            // get the max number of slots
            int max = slots.Count + numOfSlots;

            bool firstIteration = true;

            while (true)
            {
                for (int x = firstIteration ? startX : 0; x < SlotsInRow; x++)
                {
                    GameObject slot = Instantiate(slotPrefab, contentParent.transform);

                    ManageSlotOnLoad(slot, slotIndex);

                    Image slotImg = slot.GetComponent<Image>();

                    slotImg.rectTransform.anchoredPosition = new Vector3(x * offset + firstSlotPos.x, y * -offset + firstSlotPos.y);


                    slots.Add(slotImg);

                    slotIndex++;

                    if (slotIndex >= max)
                    {
                        return;
                    }
                }
                firstIteration = false;
                y++;
            }
        }
    }
}
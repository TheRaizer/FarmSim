using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using FarmSim.Utility;

namespace FarmSim.Player 
{

    public class InventoryUI : MonoBehaviour
    {
        [SerializeField] private GameObject slotPrefab;
        [SerializeField] private GameObject contentParent;

        private ObjectPooler objectPooler;

        public List<Image> Slots { get; private set; } = new List<Image>();

        // first slot is the top left slot
        private const int FIRST_SLOT_X = -87;
        private const int FIRST_SLOT_Y = 331;

        private const int OFFSET = 60;

        private void Awake()
        {
            objectPooler = GetComponent<ObjectPooler>();
        }

        public void InitializeSlots(int numOfSlots, List<Item> inventory)
        {
            int y = 0;
            int slotIndex = 0;

            while (true)
            {
                for (int x = 0; x < 4; x++)
                {

                    GameObject slot = Instantiate(slotPrefab);
                    Image slotImg = slot.GetComponent<Image>();

                    slotImg.transform.SetParent(contentParent.transform);
                    // initialize slotImg scale and position
                    slotImg.rectTransform.localScale = new Vector3(2.0197f, 2.0197f, 2.0197f);
                    slotImg.rectTransform.anchoredPosition = new Vector2(x * OFFSET + FIRST_SLOT_X, y * -OFFSET + FIRST_SLOT_Y);

                    // if the inventory has an item that can be slotted
                    if (slotIndex < inventory.Count)
                    {
                        // get the item and spawn a placeableSpawner at the slot
                        Item item = inventory[slotIndex];
                        GameObject placeableSpawner = objectPooler.SpawnGameObject(item.itemType.ItemName + "Spawner", Vector2.zero, Quaternion.identity);
                        Image placeableSpawnerImg = placeableSpawner.GetComponent<Image>();

                        // assign the placeable spawner to the item
                        item.PlaceableSpawner = placeableSpawnerImg;
                        placeableSpawner.transform.SetParent(slotImg.transform);
                        placeableSpawnerImg.rectTransform.anchoredPosition = Vector3.zero;
                    }

                    Slots.Add(slotImg);

                    slotIndex++;

                    if (slotIndex > numOfSlots)
                        return;
                }
                y++;
            }
        }
    }
}
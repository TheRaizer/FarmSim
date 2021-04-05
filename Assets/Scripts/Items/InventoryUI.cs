using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

namespace FarmSim.Items
{
    /// <class name="InventoryUI">
    ///     <summary>
    ///         Controls the slots in the inventory UI as well as the <see cref="Image"/>'s within the slots.
    ///     </summary>
    /// </class>
    public class InventoryUI : InventorySlotsHandler
    {
        [SerializeField] private GameObject inventoryUI;
        public bool IsActive { get; private set; } = false;

        protected override void Awake()
        {
            base.Awake();
            inventoryUI.SetActive(false);
        }
        private void LateUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                IsActive = !IsActive;
                inventoryUI.SetActive(IsActive);
            }
        }

        public void InitializeUI(int numSlots, List<Item> inventory)
        {
            LoadSlots(numSlots);
            LoadItemImagesIntoSlots(inventory);
        }

        private void LoadItemImagesIntoSlots(List<Item> inventory)
        {
            inventory.ForEach(item =>
            {
                Image slotImg = slots[item.SlotIndex].GetComponent<Image>();
                SpawnImage(item, slotImg, item.SlotIndex);
            });
        }
    }
}
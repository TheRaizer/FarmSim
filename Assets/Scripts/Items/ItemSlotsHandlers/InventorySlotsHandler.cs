﻿using FarmSim.Slots;
using FarmSim.Utility;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FarmSim.Items
{
    /// <class name="InventorySlotsHandler">
    ///     <summary>
    ///         Manages the slots of a given content panel relating to the inventories UI.
    ///     </summary>
    /// </class>
    public class InventorySlotsHandler : SlotsHandler
    {
        [SerializeField] private GameObject inventoryUI;

        public Action OnInventoryClosed { private get; set; }

        protected override void Awake()
        {
            base.Awake();
            inventoryUI.SetActive(false);
        }

        private void LateUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                inventoryUI.SetActive(!inventoryUI.activeInHierarchy);
                if (!inventoryUI.activeInHierarchy)
                {
                    OnInventoryClosed?.Invoke();
                }
            }
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

        protected override void ManageSlotOnLoad(GameObject slot, int slotIndex)
        {
            // assign click manager properties
            var slotManager = slot.GetComponent<EmptySlotManager>();
            slotManager.SlotIndex = slotIndex;
            slotManager.SlotsHandler = this;
        }

        protected void SpawnImage(Item item, Image slotImg, int slotIndex)
        {
            GameObject itemObj = item.SpawnInventoryIcon(slotIndex, this);
            itemObj.transform.SetParent(slotImg.transform);
            var rect = itemObj.GetComponent<RectTransform>();

            rect.Center();

            // reset its scale to 1
            rect.localScale = Vector3.one;
        }

        public void InitializeUI(int numSlots, List<Item> inventory)
        {
            AddSlots(numSlots);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace FarmSim.Items
{
    public class ItemTrash : MonoBehaviour, IPointerClickHandler
    {
        private ItemMovementManager itemMovementManager;

        private void Awake()
        {
            itemMovementManager = FindObjectOfType<ItemMovementManager>();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if(eventData.button == PointerEventData.InputButton.Left)
            {
                itemMovementManager.DestroyAttachedItem();
            }
        }
    }
}

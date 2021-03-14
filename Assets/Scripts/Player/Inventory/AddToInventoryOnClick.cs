using UnityEngine;

namespace FarmSim.Player {
    public class AddToInventoryOnClick : MonoBehaviour
    {
        [SerializeField] private ItemType itemType;
        [SerializeField] private int amt;

        private PlayerInventory inventory;

        private void Awake()
        {
            inventory = FindObjectOfType<PlayerInventory>();
        }

        private void OnMouseDown()
        {
            inventory.AddToInventory(itemType, amt);
        }
    }
}
using UnityEngine;

namespace FarmSim.Player {
    public class AddToInventoryOnClick : MonoBehaviour
    {
        [SerializeField] private ItemType itemType;
        [SerializeField] private int amt;

        private PlayerInventoryList inventory;

        private void Awake()
        {
            inventory = FindObjectOfType<PlayerInventoryList>();
        }

        private void OnMouseDown()
        {
            inventory.AddToInventory(itemType, amt);
        }
    }
}
using UnityEngine;

namespace FarmSim.Items
{
    public class AddToInventoryOnClick : MonoBehaviour
    {
        [SerializeField] private ItemType itemType;
        [SerializeField] private int amt;

        private Inventory inventory;

        private void Awake()
        {
            inventory = FindObjectOfType<Inventory>();
        }

        private void OnMouseDown()
        {
            inventory.AddToInventory(itemType, amt);
        }
    }
}
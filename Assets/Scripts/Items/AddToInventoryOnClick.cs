using UnityEngine;

namespace FarmSim.Items
{
    public class AddToInventoryOnClick : MonoBehaviour
    {
        [SerializeField] private ItemType itemType;
        [SerializeField] private int amt;

        private void OnMouseOver()
        {
            if (Input.GetMouseButtonDown(1))
            {
                itemType.SpawnWorldItem(transform.position, amt);
            }
        }
    }
}
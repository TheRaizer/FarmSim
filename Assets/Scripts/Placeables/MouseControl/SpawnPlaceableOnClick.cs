using FarmSim.Player;
using FarmSim.Utility;
using UnityEngine;

namespace FarmSim.Placeable 
{
    public class SpawnPlaceableOnClick : MonoBehaviour
    {
        [SerializeField] private ItemType itemType;

        private MoveObject moveObject = null;
        private ObjectPooler objectPooler = null;

        private SpriteRenderer spriteRenderer;
        private PlayerInventory inventory;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();

            moveObject = FindObjectOfType<MoveObject>();
            inventory = FindObjectOfType<PlayerInventory>();
            objectPooler = FindObjectOfType<ObjectPooler>();
        }

        private void OnMouseDown()
        {
            SpawnPlaceable();
        }

        /// <summary>
        ///     Spawns some placeable object given the itemType and attaches it to the mouse.
        ///     If there is already and existing placeable attached to the mouse, set it unactive.
        /// </summary>
        private void SpawnPlaceable()
        {
            Item item = inventory.GetItem(itemType);

            if (item == null || item.Amt <= 0)
            {
                return;
            }

            // Spawn the items corrosponding placeable object
            GameObject objToAttach = objectPooler.SpawnGameObject(itemType.ItemName, Vector2.zero, Quaternion.identity);

            if (objToAttach.TryGetComponent(out Placeable placeable))
            {
                if (moveObject.AttachedObject != null)
                {
                    // if there is currently an attached object then because attached objects are spawned from object pooler set it unactive
                    moveObject.AttachedObject.gameObject.SetActive(false);
                }
                // assign a new attached object
                moveObject.AttachedObject = placeable;
            }

            // subtract from the items amount
            item.SubtractFromAmt(1);

            if (item.Amt == 0)
            {
                // make sprite dark or smthn
            }
        }
    }
}

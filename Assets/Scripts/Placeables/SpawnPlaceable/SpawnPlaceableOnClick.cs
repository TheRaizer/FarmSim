using FarmSim.Player;
using FarmSim.Utility;
using FarmSim.Enums;
using UnityEngine;

namespace FarmSim.Placeables 
{
    public class SpawnPlaceableOnClick : MonoBehaviour
    {
        /// <summary>
        ///     Points to some item in the inventory and will be passed to the placeable.
        /// </summary>
        [SerializeField] private ItemType itemType;

        private MoveObject moveObject = null;
        private ObjectPooler objectPooler = null;

        private SpriteRenderer spriteRenderer = null;
        private PlayerInventory inventory;
        private ToolHandler toolHandler;
        private Item item;

        private Color baseColor;

        private void Awake()
        {
            moveObject = FindObjectOfType<MoveObject>();
            inventory = FindObjectOfType<PlayerInventory>();
            objectPooler = FindObjectOfType<ObjectPooler>();
            toolHandler = FindObjectOfType<ToolHandler>();
            spriteRenderer = GetComponent<SpriteRenderer>();

            baseColor = spriteRenderer.color;
        }

        private void Update()
        {
            if(item == null)
            {
                item = inventory.GetItem(itemType);
            }
            // Probably need to optimize this.
            ChangeSpriteColor(item);

            if(toolHandler.EquippedTool.ToolType != ToolTypes.Hand)
            {
                RemoveCurrentPlaceable(null);
            }
        }
        private void OnMouseOver()
        {
            if (Input.GetMouseButtonDown(1))
            {
                // only if the current tool equipped is the hand should we spawn a placeable
                if (toolHandler.EquippedTool.ToolType == ToolTypes.Hand)
                {
                    SpawnPlaceable();
                }
            }
        }

        private void ChangeSpriteColor(Item item)
        {
            if (item == null)
            {
                spriteRenderer.color = Color.black;
            }
            else if (!item.CanSubtract)
            {
                spriteRenderer.color = Color.grey;
            }
            else
            {
                spriteRenderer.color = baseColor;
            }
        }

        /// <summary>
        ///     Spawns some placeable object given the itemType and attaches it to the mouse.
        ///     If there is already and existing placeable attached to the mouse, set it unactive.
        /// </summary>
        private void SpawnPlaceable()
        {
            Item item = inventory.GetItem(itemType);

            if (item == null || !item.CanSubtract)
            {
                ChangeSpriteColor(item);
                return;
            }

            // Spawn the items corrosponding placeable object
            GameObject objToAttach = objectPooler.SpawnGameObject(itemType.ItemName, Vector2.zero, Quaternion.identity);

            if (objToAttach.TryGetComponent(out Placeable placeable))
            {
                placeable.Item = item;
                bool setNewPlaceable = RemoveCurrentPlaceable(objToAttach);
                if (!setNewPlaceable)
                {
                    return;
                }
                // assign a new attached object
                moveObject.AttachedObject = placeable;
                placeable.FirstClick = false;
            }
        }

        private bool RemoveCurrentPlaceable(GameObject objToAttach)
        {
            bool setNewPlaceable = true;
            if (moveObject.AttachedObject != null)
            {
                // if there is currently an attached object then because attached objects are spawned from object pooler set it unactive
                moveObject.AttachedObject.gameObject.SetActive(false);

                // if we click on the same object then dont set the new attached object.
                if (moveObject.AttachedObject.gameObject == objToAttach)
                {
                    setNewPlaceable = false;
                }

                moveObject.AttachedObject = null;
            }
            return setNewPlaceable;
        }
    }
}

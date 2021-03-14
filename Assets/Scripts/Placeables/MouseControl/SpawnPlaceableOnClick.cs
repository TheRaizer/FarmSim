using FarmSim.Loading;
using FarmSim.Player;
using FarmSim.Utility;
using UnityEngine;

namespace FarmSim.Placeable 
{
    public class SpawnPlaceableOnClick : OccurPostLoad
    {
        /// <summary>
        ///     Points to some item in the inventory and will be passed to the placeable.
        /// </summary>
        [SerializeField] private ItemType itemType;

        private MoveObject moveObject = null;
        private ObjectPooler objectPooler = null;

        private SpriteRenderer spriteRenderer = null;
        private PlayerInventory inventory;
        private Item item;

        private Color baseColor;

        protected override void Awake()
        {
            base.Awake();

            moveObject = FindObjectOfType<MoveObject>();
            inventory = FindObjectOfType<PlayerInventory>();
            objectPooler = FindObjectOfType<ObjectPooler>();
            spriteRenderer = GetComponent<SpriteRenderer>();

            baseColor = spriteRenderer.color;
        }

        protected override void Update()
        {
            base.Update();

            // Probably need to optimize this.
            ChangeSpriteColor(item);
        }

        private void OnMouseDown()
        {
            SpawnPlaceable();
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
                if (moveObject.AttachedObject != null)
                {
                    // if there is currently an attached object then because attached objects are spawned from object pooler set it unactive
                    moveObject.AttachedObject.gameObject.SetActive(false);

                    // if we click on the same object then dont set the new attached object.
                    if(moveObject.AttachedObject.gameObject == objToAttach)
                    {
                        moveObject.AttachedObject = null;
                        return;
                    }
                }
                // assign a new attached object
                moveObject.AttachedObject = placeable;
            }
        }

        protected override void PostLoad()
        {
            item = inventory.GetItem(itemType);
        }
    }
}

using FarmSim.Player;
using FarmSim.Utility;
using FarmSim.Enums;
using UnityEngine;
using UnityEngine.EventSystems;

namespace FarmSim.Placeables 
{
    /// <class name="SpawnPlaceableOnClick">
    ///     <summary>
    ///         Spawns a Placeable object and references a <see cref="Item"/> through its GUUID.
    ///     </summary>
    /// </class>
    public class SpawnPlaceableOnClick : MonoBehaviour, IPointerClickHandler, IReferenceGUID
    {
        private MoveObject moveObject = null;
        private ObjectPooler objectPooler = null;

        private PlayerInventoryList inventory;
        private ToolHandler toolHandler;

        /// <summary>
        ///     Points to some item in the inventory and will be passed to the placeable.
        /// </summary>
        public string Guid { get; set; }

        private void Awake()
        {
            moveObject = FindObjectOfType<MoveObject>();
            inventory = FindObjectOfType<PlayerInventoryList>();
            objectPooler = FindObjectOfType<ObjectPooler>();
            toolHandler = FindObjectOfType<ToolHandler>();
        }

        private void Update()
        {
            if(toolHandler.EquippedTool.ToolType != ToolTypes.Hand)
            {
                RemoveCurrentPlaceable(null);
            }
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                // only if the current tool equipped is the hand should we spawn a placeable
                if (toolHandler.EquippedTool.ToolType == ToolTypes.Hand)
                {
                    SpawnPlaceable();
                }
            }
        }

        /// <summary>
        ///     Spawns some placeable object given the itemType and attaches it to the mouse.
        ///     If there is already and existing placeable attached to the mouse, set it unactive.
        /// </summary>
        private void SpawnPlaceable()
        {
            Item item = inventory.TakeFromInventory(Guid, 0);

            // Spawn the items corrosponding placeable object
            GameObject objToAttach = objectPooler.SpawnGameObject(item.itemType.ItemName, Vector2.zero, Quaternion.identity);

            if (objToAttach.TryGetComponent(out Placeable placeable))
            {
                placeable.Guid = Guid;
                bool setNewPlaceable = RemoveCurrentPlaceable(objToAttach);
                if (!setNewPlaceable)
                {
                    return;
                }
                // assign a new attached object
                moveObject.AttachedObject = placeable;
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

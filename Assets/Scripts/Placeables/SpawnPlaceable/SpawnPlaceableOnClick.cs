using FarmSim.Enums;
using FarmSim.Items;
using FarmSim.Player;
using FarmSim.Utility;
using UnityEngine;
using UnityEngine.EventSystems;

namespace FarmSim.Placeables
{
    /// <class name="SpawnPlaceableOnClick">
    ///     <summary>
    ///         Spawns a Placeable object and references a <see cref="Item"/> through its GUUID.
    ///     </summary>
    /// </class>
    public class SpawnPlaceableOnClick : MonoBehaviour, IPointerClickHandler, IItemRefsGUID
    {
        private MovePlaceable movePlaceable = null;
        private ObjectPooler objectPooler = null;

        private Inventory inventory;
        private ToolHandler toolHandler;

        /// <summary>
        ///     Points to some item in the inventory and will be passed to the placeable.
        /// </summary>
        public string itemGuid { get; set; }

        private void Awake()
        {
            movePlaceable = FindObjectOfType<MovePlaceable>();
            inventory = FindObjectOfType<Inventory>();
            objectPooler = FindObjectOfType<ObjectPooler>();
            toolHandler = FindObjectOfType<ToolHandler>();
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
            Item item = inventory.GetExactItem(itemGuid);

            // assign an action to the item that will be called when its destroyed in order to also remove the attached placeable if it matches
            item.RemoveAttachedPlaceableIfMatching = RemoveAttachedSwappableIfMatching;

            // Spawn the items corrosponding placeable object
            GameObject objToAttach = objectPooler.SpawnGameObject(item.itemType.ItemName, Vector2.zero, Quaternion.identity);

            if (objToAttach.TryGetComponent(out Placeable placeable))
            {
                placeable.itemGuid = itemGuid;
                bool setNewPlaceable = RemoveCurrentPlaceable(objToAttach);

                if (!setNewPlaceable)
                {
                    return;
                }
                // assign a new attached object
                movePlaceable.AttachedPlaceable = placeable;
            }
        }

        private bool RemoveCurrentPlaceable(GameObject objToAttach)
        {
            bool setNewPlaceable = true;
            if (movePlaceable.AttachedPlaceable != null)
            {

                // if we click on the same object then dont set the new attached object.
                if (movePlaceable.AttachedPlaceable.gameObject == objToAttach)
                {
                    setNewPlaceable = false;
                }

                movePlaceable.RemoveAttachedPlaceable();
            }
            return setNewPlaceable;
        }

        private void RemoveAttachedSwappableIfMatching(string guid)
        {
            string attachedGuid = movePlaceable.AttachedPlaceable.itemGuid;
            if (attachedGuid == guid)
            {
                movePlaceable.RemoveAttachedPlaceable();
            }
        }
    }
}

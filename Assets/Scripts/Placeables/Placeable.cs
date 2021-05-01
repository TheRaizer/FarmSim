using FarmSim.Grid;
using FarmSim.Items;
using FarmSim.Player;
using FarmSim.Serialization;
using UnityEngine;
using UnityEngine.EventSystems;

namespace FarmSim.Placeables
{
    /// <class name="Placeable">
    ///     <summary>
    ///         The base class for any placeable gameObject. 
    ///         Manages the placement of a given object.
    ///     </summary>
    /// </class>
    public class Placeable : MonoBehaviour, IItemRefsGUID
    {
        [SerializeField] protected int xDim = 0;
        [SerializeField] protected int yDim = 0;
        [SerializeField] protected bool isWalkable = true;
        [SerializeField] protected GameObject objectToPlace;

        public INodeData DestinationNodeData { get; set; }

        /// <summary>
        ///     This guid is given when the placeable is spawned.
        ///     The relating item amount should be reduced whenever a placement was succesful.
        /// </summary>
        public string ItemGuid { get; set; }
        protected PlayerController player;
        protected NodeGrid nodeGrid;
        private Inventory inventory;

        private SpriteRenderer sprite = null;
        private MovePlaceable movePlaceable = null;

        private Color invalidColor;
        private Color validColor;

        public bool CanBePlaced(INodeData nodeData) => nodeData != null && nodeGrid.IsValidPlacement(nodeData, xDim, yDim);

        protected virtual void Awake()
        {
            if (xDim % 2 == 0 || yDim % 2 == 0)
            {
                Debug.LogError("Center node cannot be found with even dimensions");
            }
            sprite = GetComponent<SpriteRenderer>();
            movePlaceable = FindObjectOfType<MovePlaceable>();
            player = FindObjectOfType<PlayerController>();
            inventory = FindObjectOfType<Inventory>();
            nodeGrid = FindObjectOfType<NodeGrid>();

            InitColors();
        }


        protected virtual void Update()
        {
            if (Input.GetMouseButtonDown(1) && !EventSystem.current.IsPointerOverGameObject())
            {
                if (CanBePlaced(DestinationNodeData))
                {
                    nodeGrid.MakeDimensionsOccupied(DestinationNodeData, xDim, yDim, isWalkable);
                    OnPlace();
                }
            }
        }

        /// <summary>
        ///     Initialize the colors for valid and invalid placements
        /// </summary>
        private void InitColors()
        {
            invalidColor = new Color(sprite.color.r, sprite.color.g / 2, sprite.color.b / 2, sprite.color.a / 2);
            validColor = sprite.color;
        }

        /// <summary>
        ///     Changes the position of the placeable.
        /// </summary>
        /// <param name="newPos">The new position to assign to the placeable.</param>
        public void ChangePosition(Vector2 newPos)
        {
            transform.position = newPos;

            if (!CanBePlaced(DestinationNodeData))
            {
                sprite.color = invalidColor;
            }
            else
            {
                sprite.color = validColor;
            }
        }


        /// <summary>
        ///     Places <see cref="objectToPlace"/> at the current node.
        /// </summary>
        protected virtual void OnPlace()
        {
            ReduceAmtPlaceable();
            var obj = Instantiate(objectToPlace);
            obj.transform.position = DestinationNodeData.Data.pos;
        }

        protected void ReduceAmtPlaceable()
        {
            Item item = inventory.TakeFromInventory(ItemGuid, 1);
            if (item != null && item.Amt <= 0)
            {
                movePlaceable.RemoveAttachedPlaceable();
            }
        }
    }
}


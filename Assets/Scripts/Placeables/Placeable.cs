using UnityEngine;
using FarmSim.Grid;
using FarmSim.Player;

namespace FarmSim.Placeables
{
    /// <class name="Placeable">
    ///     <summary>
    ///         The base class for any placeable gameObject. 
    ///         Manages the placement of a given object.
    ///     </summary>
    /// </class>
    public class Placeable : MonoBehaviour
    {
        [SerializeField] protected int xDim = 0;
        [SerializeField] protected int yDim = 0;
        [SerializeField] protected bool isWalkable = true;
        [SerializeField] protected GameObject objectToPlace;

        /// <summary>
        ///     This itemType is given when the placeable is spawned.
        ///     The relating item amount should be reduced whenever a placement was succesful.
        /// </summary>
        public ItemType ItemType { protected get; set; }

        // REPLACE THIS COMPLETELY WITH PLAYER DESTINATION NODE
        public Node Node { get; set; }

        protected NodeGrid grid = null;
        protected PlayerController player;
        private PlayerInventory inventory;

        private SpriteRenderer sprite = null;
        private MoveObject moveObject = null;

        private Color invalidColor;
        private Color validColor;

        protected virtual void Awake()
        {
            if (xDim % 2 == 0 || yDim % 2 == 0)
            {
                Debug.LogError("Center node cannot be found with even dimensions");
            }
            grid = FindObjectOfType<NodeGrid>();
            sprite = GetComponent<SpriteRenderer>();
            moveObject = FindObjectOfType<MoveObject>();
            player = FindObjectOfType<PlayerController>();
            inventory = FindObjectOfType<PlayerInventory>();

            InitColors();
        }


        protected virtual void Update()
        {
            if (Input.GetMouseButtonDown(1))
            {
                if (grid.IsValidPlacement(Node, xDim, yDim))
                {
                    grid.MakeDimensionsOccupied(Node, xDim, yDim, isWalkable);
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

            if (!grid.IsValidPlacement(Node, xDim, yDim))
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
            var obj = Instantiate(objectToPlace);
            obj.transform.position = Node.Data.pos;
        }

        protected void ReduceAmtPlaceable()
        {
            Item item = inventory.TakeFromInventory(ItemType, 1);
            if (item != null && item.Amt <= 0)
            {
                moveObject.AttachedObject = null;
                gameObject.SetActive(false);
            }
        }
    }
}


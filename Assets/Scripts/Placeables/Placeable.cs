using UnityEngine;
using FarmSim.Grid;
using FarmSim.Player;

namespace FarmSim.Placeable
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
        [SerializeField] protected GameObject objectToPlace;

        /// <summary>
        ///     This item is given when the placeable is spawned.
        ///     The amount should be reduced whenever a placement was succesful.
        /// </summary>
        public Item Item { protected get; set; }
        public Node Node { get; set; }

        protected NodeGrid grid = null;

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

            InitColors();
        }


        protected virtual void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (grid.IsValidPlacement(Node, xDim, yDim))
                {
                    grid.MakeDimensionsOccupied(Node, xDim, yDim);
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
            if (Item.CanSubtract)
            {
                Item.SubtractFromAmt(1);
                if (Item.Amt <= 0)
                {
                    moveObject.AttachedObject = null;
                    gameObject.SetActive(false);
                }
            }
        }
    }
}


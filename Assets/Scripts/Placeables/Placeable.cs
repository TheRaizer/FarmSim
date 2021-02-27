using UnityEngine;
using FarmSim.Grid;


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

    public Node Node { get; set; }

    private SpriteRenderer sprite = null;
    private NodeGrid grid = null;
    private Color invalidColor;
    private Color validColor;

    protected virtual void Awake()
    {
        if(xDim % 2 == 0 || yDim % 2 == 0)
        {
            Debug.LogError("Center node cannot be found with even dimensions");
        }
        grid = FindObjectOfType<NodeGrid>();
        sprite = GetComponent<SpriteRenderer>();

        AddToCurrentMovingObject();
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
    ///     Add this placeable to be the current moving object.
    /// </summary>
    private void AddToCurrentMovingObject()
    {
        var moveObject = FindObjectOfType<MoveObject>();
        moveObject.AttachedObject = this;
    }

    /// <summary>
    ///     Changes the position of the placeable.
    /// </summary>
    /// <param name="newPos">The new position to assign to the placeable.</param>
    public void ChangePosition(Vector2 newPos)
    {
        transform.position = newPos;

        if(!grid.IsValidPlacement(Node, xDim, yDim))
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
        obj.transform.position = Node.Position;
    }
}

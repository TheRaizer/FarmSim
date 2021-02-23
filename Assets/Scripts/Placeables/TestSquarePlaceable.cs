using FarmSim.Grid;
using UnityEngine;

public class TestSquarePlaceable : MonoBehaviour, IPlaceable
{
    [field: SerializeField] public int XDim { get; }
    [field: SerializeField] public int YDim { get; }

    public Node Node { get; set; }
    public Vector2 Pos { get; set; }

    [field: SerializeField] public GameObject ObjectToPlace { get; private set; }

    private void Awake()
    {
        var moveObject = FindObjectOfType<MoveObject>();
        moveObject.AttachedObject = this;
    }


    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnPlace();
        }
        ChangePosition();
    }

    public void OnPlace()
    {
        Debug.Log("Place " + ObjectToPlace + " at " + Node);
    }

    public void ChangePosition()
    {
        transform.position = Pos;
    }
}

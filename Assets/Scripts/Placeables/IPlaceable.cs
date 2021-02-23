using UnityEngine;
using FarmSim.Grid;

public interface IPlaceable
{
    int XDim { get; }
    int YDim { get; }

    Node Node { get; set; }
    Vector2 Pos { get; set;  }
    GameObject ObjectToPlace { get; }


    void ChangePosition();
    void OnPlace();
}

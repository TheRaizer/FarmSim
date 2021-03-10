
using FarmSim.Enums;
using UnityEngine;

namespace FarmSim.Placeable
{
    public class PlacePlant : Placeable
    {

        protected override void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (grid.IsValidPlacement(Node, xDim, yDim))
                {
                    OnPlace();
                }
            }
        }

        protected override void OnPlace()
        {
            Node.Interactable.OnInteract(ToolTypes.Other, objectToPlace, () => grid.MakeDimensionsOccupied(Node, xDim, yDim));
        }
    }
}

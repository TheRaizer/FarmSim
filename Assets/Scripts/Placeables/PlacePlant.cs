using FarmSim.Enums;
using FarmSim.Grid;
using UnityEngine;

namespace FarmSim.Placeables
{
    /// <class name="PlacePlant">
    ///     <summary>
    ///         Manages the placement of a Plant.
    ///         <remarks>
    ///             <para>This class works using some IInteractable.</para>
    ///             <para>This means that it will only occupy space if the interaction was succesful unlike its base Placeable class.</para>
    ///         </remarks>
    ///     </summary>
    /// </class>
    public class PlacePlant : Placeable
    {
        Node destination = null;

        protected override void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                destination = NodeGrid.Instance.GetNodeFromMousePosition();

                // unlike in the Placeable parent class we do not want to occupy the space unless the interaction was succesful.
                if (destination != null && NodeGrid.Instance.IsValidPlacement(destination, xDim, yDim))
                {
                    player.OnPlant = OnPlace;
                }
            }
        }

        protected override void OnPlace()
        {
            // use the Node OnInteract in an attempt to spawn a planteable on a instance of Dirt.
            destination.Interactable.OnInteract(ToolTypes.Other, objectToPlace, OnPlantingSuccesful);
        }

        private void OnPlantingSuccesful()
        {
            NodeGrid.Instance.MakeDimensionsOccupied(destination, xDim, yDim, isWalkable);
            ReduceAmtPlaceable();
        }
    }
}

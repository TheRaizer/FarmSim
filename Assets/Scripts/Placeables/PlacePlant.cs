
using FarmSim.Enums;
using UnityEngine;

namespace FarmSim.Placeable
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
        protected override void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                // unlike in the Placeable parent class we do not want to occupy the space unless the interaction was succesful.
                if (grid.IsValidPlacement(Node, xDim, yDim))
                {
                    OnPlace();
                }
            }
        }

        protected override void OnPlace()
        {
            Node.Interactable.OnInteract(ToolTypes.Other, objectToPlace, 
                () => 
                { 
                    grid.MakeDimensionsOccupied(Node, xDim, yDim);
                    ReduceAmtPlaceable();
                });
        }
    }
}

using FarmSim.Player;
using UnityEngine;

namespace FarmSim.Placeables
{
    /// <class name="RemoveAttachedPlaceable">
    ///     <summary>
    ///         Removes the attached placeable if it is not the hand.
    ///     </summary>
    /// </class>
    public class RemoveAttachedPlaceable : MonoBehaviour
    {
        private MovePlaceable movePlaceable;
        private ToolHandler toolHandler;

        private void Awake()
        {
            toolHandler = FindObjectOfType<ToolHandler>();
            movePlaceable = GetComponent<MovePlaceable>();
        }

        private void Update()
        {
            if (toolHandler.EquippedTool.ToolType != Enums.ToolTypes.Hand)
            {
                movePlaceable.RemoveAttachedPlaceable();
            }
        }
    }
}

using FarmSim.Player;
using UnityEngine;

namespace FarmSim.Placeables
{
    /// <class name="PlayerInventoryList">
    ///     <summary>
    ///         Removes the attached placeable if it is not the hand.
    ///     </summary>
    /// </class>
    public class RemoveAttachedPlaceable : MonoBehaviour
    {
        private MoveObject moveObject;
        private ToolHandler toolHandler;

        private void Awake()
        {
            toolHandler = FindObjectOfType<ToolHandler>();
            moveObject = GetComponent<MoveObject>();
        }

        private void Update()
        {
            if (toolHandler.EquippedTool.ToolType != Enums.ToolTypes.Hand && moveObject.AttachedObject != null)
            {
                moveObject.AttachedObject.gameObject.SetActive(false);
                moveObject.AttachedObject = null;
            }
        }
    }
}

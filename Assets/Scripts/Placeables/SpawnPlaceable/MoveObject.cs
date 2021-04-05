using UnityEngine;
using FarmSim.Grid;


namespace FarmSim.Placeables
{
    /// <class name="MoveObject">
    ///     <summary>
    ///         Manages the movement of a single IPlaceable object at a time.
    ///     </summary>
    /// </class>
    public class MoveObject : MonoBehaviour
    {
        public Placeable AttachedObject { get; set; } = null;
        private Node currentNode = null;

        private void Update()
        {
            if (AttachedObject != null)
            {
                MovePlaceableToNode();
            }
        }

        /// <summary>
        ///     Moves some IPlaceable to its closest node.
        /// </summary>
        private void MovePlaceableToNode()
        {
            Node node = NodeGrid.Instance.GetNodeFromMousePosition();

            if (node != null && (Vector2)AttachedObject.transform.position != node.Data.pos)
            {
                currentNode = node;

                AttachedObject.Node = currentNode;
                AttachedObject.ChangePosition(currentNode.Data.pos);
            }
        }
    }
}
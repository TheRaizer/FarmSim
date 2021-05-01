using FarmSim.Grid;
using FarmSim.Serialization;
using UnityEngine;

namespace FarmSim.Placeables
{
    /// <class name="MovePlaceable">
    ///     <summary>
    ///         Manages the movement of a single IPlaceable object at a time.
    ///     </summary>
    /// </class>
    public class MovePlaceable : MonoBehaviour
    {
        public Placeable AttachedPlaceable { get; set; } = null;
        private INodeData currentNode = null;
        private NodeGrid nodeGrid;

        private void Awake()
        {
            nodeGrid = FindObjectOfType<NodeGrid>();
        }

        private void Update()
        {
            if (AttachedPlaceable != null)
            {
                MovePlaceableToNode();
            }
        }

        public void RemoveAttachedPlaceable()
        {
            if (AttachedPlaceable != null)
            {
                AttachedPlaceable.gameObject.SetActive(false);
                AttachedPlaceable = null;
            }
        }

        /// <summary>
        ///     Moves some IPlaceable to its closest node.
        /// </summary>
        private void MovePlaceableToNode()
        {
            INodeData node = nodeGrid.GetNodeFromMousePosition();


            if (node != null && (Vector2)AttachedPlaceable.transform.position != node.Data.pos)
            {
                if (!AttachedPlaceable.CanBePlaced(node))
                    return;

                currentNode = node;

                AttachedPlaceable.DestinationNodeData = currentNode;
                AttachedPlaceable.ChangePosition(currentNode.Data.pos);
            }
        }
    }
}
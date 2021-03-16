using FarmSim.Serialization;

namespace FarmSim.Grid
{
    /// <class name="Node">
    ///     <summary>
    ///         Manages the positions of game objects as well as 
    ///         where game objects can be placed depending on if a 
    ///         Node is occupied or not.
    ///     </summary>
    /// </class>
    public class Node : IHeapItem<Node>
    {
        public const float NODE_RADIUS = 0.7f;
        public const float NODE_DIAMETER = NODE_RADIUS * 2;

        public NodeData Data { get; private set; }
        public IInteractable Interactable { get; set; }
        public int HeapIndex { get; set; }

        public Node parentNode;
        public int hCost = 0;
        public int gCost = 0;

        public int FCost
        {
            get
            {
                return gCost + hCost;
            }
        }

        /// <param name="position">The world position of the node</param>
        /// <param name="x">The x-indices of the node</param>
        /// <param name="y">The y-indices of the node</param>
        public Node(NodeData _data)
        {
            Data = _data;
        }

        public int CompareTo(Node nodeToCompare)
        {
            int compare = FCost.CompareTo(nodeToCompare.FCost);
            if (compare == 0)
            {
                compare = hCost.CompareTo(nodeToCompare.hCost);
            }
            return -compare;
        }
    }
}

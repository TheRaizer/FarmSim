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
    public class Node : IHeapItem<Node>, INodeData
    {
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

        /// <summary>
        ///     Compares two nodes to determine their priority in a heap.
        ///     <remarks><see cref="int.CompareTo(int)"/> will return 1 if FCost/HCost is greater which is why we return (-compare).</remarks>
        /// </summary>
        /// <param name="other"></param>
        /// <returns>Negative compare as we are prioritizing when FCost or HCost is lower.</returns>
        public int CompareTo(Node other)
        {
            int compare = FCost.CompareTo(other.FCost);
            if (compare == 0)
            {
                compare = hCost.CompareTo(other.hCost);
            }
            return -compare;
        }

        public override string ToString()
        {
            return "X: " + Data.x + " || Y: " + Data.y;
        }
    }
}

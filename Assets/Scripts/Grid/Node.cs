﻿using FarmSim.Serialization;

namespace FarmSim.Grid
{
    /// <class name="Node">
    ///     <summary>
    ///         Manages the positions of game objects as well as 
    ///         where game objects can be placed depending on if a 
    ///         Node is occupied or not.
    ///     </summary>
    /// </class>
    public class Node
    {
        public const float NODE_RADIUS = 0.7f;
        public const float NODE_DIAMETER = NODE_RADIUS * 2;

        public NodeData Data { get; private set; }
        public IInteractable Interactable { get; set; }

        /// <param name="position">The world position of the node</param>
        /// <param name="x">The x-indices of the node</param>
        /// <param name="y">The y-indices of the node</param>
        public Node(NodeData _data)
        {
            Data = _data;
        }
    }
}

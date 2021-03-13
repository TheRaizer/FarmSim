﻿using UnityEngine;

namespace FarmSim.Serialization
{
    /// <class name="NodeData">
    ///     <summary>
    ///         Serializable data that a Node contains.
    ///     </summary>
    /// </class>
    [System.Serializable]
    public class NodeData
    {
        public bool IsOccupied { get; set; }
        public readonly Vector2 pos;

        public readonly int x;
        public readonly int y;

        public NodeData(bool _isOccupied, Vector2 _pos, int _x, int _y)
        {
            IsOccupied = _isOccupied;
            pos = _pos;
            x = _x;
            y = _y;
        }
    }
}

using UnityEngine;

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
        public bool isOccupied;
        public Vector2 pos;

        public int x;
        public int y;

        public NodeData(bool _isOccupied, Vector2 _pos, int _x, int _y)
        {
            isOccupied = _isOccupied;
            pos = _pos;
            x = _x;
            y = _y;
        }
    }
}

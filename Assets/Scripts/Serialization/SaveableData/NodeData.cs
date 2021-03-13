using UnityEngine;

namespace FarmSim.Serialization
{

    [System.Serializable]
    public class NodeData
    {
        public bool IsOccupied = false;
        public Vector2 Position;

        public readonly int x = 0;
        public readonly int y = 0;
    }
}

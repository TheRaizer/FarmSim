using UnityEngine;

namespace FarmSim.SavableData
{
    [System.Serializable]
    public class WorldItemData
    {
        public Vector2 Pos { get; set; }
        public int DaysActive { get; set; }
        public readonly int amt;
        public readonly string prefabName;

        public WorldItemData(int _amt, Vector2 pos, string _prefabName)
        {
            amt = _amt;
            Pos = pos;
            prefabName = _prefabName;
        }
    }

    public interface IWorldItem
    {
        public WorldItemData Data { set; }
    }
}

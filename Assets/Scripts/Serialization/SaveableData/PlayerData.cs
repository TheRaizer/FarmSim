using System.Collections.Generic;
using UnityEngine;

namespace FarmSim.Serialization
{
    /// <class name="PlayerData">
    ///     <summary>
    ///         Serializable data that the player contains.
    ///     </summary>
    /// </class>
    [System.Serializable]
    public class PlayerData
    {
        public Vector2 position = Vector2.zero;
        public List<ItemData> itemDatas;

        public int currency;

        // 0 is the starting section
        public int SectionNum = 0;

        // Singleton
        private static PlayerData _current;
        public static PlayerData Current
        {
            get
            {
                if (_current == null)
                {
                    _current = new PlayerData();
                }
                return _current;
            }
            set
            {
                if (value != null)
                {
                    _current = value;
                }
            }
        }
    }
}
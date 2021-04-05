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
    }
}
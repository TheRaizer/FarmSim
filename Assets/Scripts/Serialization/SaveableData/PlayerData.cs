using System.Collections.Generic;

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
        public List<ItemData> itemDatas;
    }
}
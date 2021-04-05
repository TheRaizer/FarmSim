using System.Collections;
using System.Collections.Generic;

namespace FarmSim.Serialization
{
    /// <class name="SaveData">
    ///     <summary>
    ///         The single object that will be saved to a file and will be accessed to retrieve loaded data.
    ///         
    ///         <remarks>
    ///             <para>You cannot serialize a static class which is why we create a singleton.</para>
    ///         </remarks>
    ///     </summary>
    /// </class>
    [System.Serializable]
    public class SaveData
    {
        // Singleton
        private static SaveData _current;

        // Data's to store and be accessed.
        public PlayerData playerData = new PlayerData();

        // collections are within lists to represent the possible sections that each collection can be part of.
        public Dictionary<int, List<DirtData>> dirtDatas = new Dictionary<int, List<DirtData>>();
        public Dictionary<int, List<PlanteableData>> plantDatas = new Dictionary<int, List<PlanteableData>>();
        public Dictionary<int, NodeData[,]> nodeDatas = new Dictionary<int, NodeData[,]>();

        public static SaveData Current
        {
            get
            {
                if (_current == null)
                {
                    _current = new SaveData();
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

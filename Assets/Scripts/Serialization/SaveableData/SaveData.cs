
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
        public List<DirtData> dirtDatas = new List<DirtData>();
        public List<PlanteableData> plantDatas = new List<PlanteableData>();
        public List<NodeData> nodeDatas = new List<NodeData>();

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

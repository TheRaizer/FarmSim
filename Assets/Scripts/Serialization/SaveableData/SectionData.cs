using System.Collections.Generic;

namespace FarmSim.Serialization
{
    /// <class name="SaveData">
    ///     <summary>
    ///         The single object that will be saved to a file and will be accessed to retrieve loaded data.
    ///         
    ///         <remarks>
    ///             <para>You cannot serialize a static class which is why we create a singleton instead.</para>
    ///         </remarks>
    ///     </summary>
    /// </class>
    [System.Serializable]
    public class SectionData
    {
        // Singleton
        private static SectionData _current;

        public int SectionNum = -1;

        // collections are within lists to represent the possible sections that each collection can be part of.
        public List<DirtData> dirtDatas = new List<DirtData>();
        public List<PlanteableData> plantDatas = new List<PlanteableData>();
        public List<TechData> techDatas = new List<TechData>();
        public NodeData[,] nodeDatas;

        public int internalDay { get; set; }

        public static SectionData Current
        {
            get
            {
                if (_current == null)
                {
                    _current = new SectionData();
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

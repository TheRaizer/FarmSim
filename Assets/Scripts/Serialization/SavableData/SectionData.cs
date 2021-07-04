using System.Collections.Generic;

namespace FarmSim.SavableData
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
        public List<DirtData> DirtDatas { get; set; } = new List<DirtData>();
        public List<PlantableData> PlantDatas { get; set; } = new List<PlantableData>();
        public List<TechData> TechDatas { get; set; } = new List<TechData>();
        public List<WorldItemData> WorldItemDatas { get; set; } = new List<WorldItemData>();
        public NodeData[,] NodeDatas { get; set; }

        public int InternalDay { get; set; }

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

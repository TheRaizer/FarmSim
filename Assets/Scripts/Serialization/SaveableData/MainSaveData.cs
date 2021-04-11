using System.Collections.Generic;

namespace FarmSim.Serialization
{
    /// <class name="MainSaveData">
    ///     <summary>
    ///         The main save of the game containing all sections.
    ///     </summary>
    /// </class>
    [System.Serializable]
    public class MainSaveData
    {
        public List<SectionData> sections = new List<SectionData>();
    }
}

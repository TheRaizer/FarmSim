using System.Collections.Generic;

namespace FarmSim.Serialization
{
    [System.Serializable]
    public class MainSaveData
    {
        public List<SectionData> sections = new List<SectionData>();
    }
}

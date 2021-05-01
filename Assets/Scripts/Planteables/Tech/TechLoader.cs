using FarmSim.Serialization;
using UnityEngine;

namespace FarmSim.Planteables 
{
    /// <class name="TechLoader">
    ///     <summary>
    ///         Loads/Instantiates all the tech objects that were saved in the given section after data has been injected into other objects
    ///     </summary>
    /// </class>
    public class TechLoader : MonoBehaviour, IOccurPostLoad
    {
        private const string TECH_PREFAB_FOLDER = "Prefabs/Tech/";
        public void PostLoad()
        {
            if (SectionData.Current.TechDatas != null)
            {
                foreach (TechData data in SectionData.Current.TechDatas)
                {
                    // get the tech prefab
                    var prefab = Resources.Load(TECH_PREFAB_FOLDER + data.prefabName) as GameObject;

                    if (prefab == null)
                    {
                        Debug.LogError("There is no tech prefab at path: " + TECH_PREFAB_FOLDER + data.prefabName);
                    }

                    var gameObject = Instantiate(prefab);
                    gameObject.transform.position = data.pos;

                    gameObject.GetComponent<ITechData>().Data = data;
                }
            }
        }
    }
}

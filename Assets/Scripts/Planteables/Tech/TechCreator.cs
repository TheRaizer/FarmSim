using FarmSim.Serialization;
using UnityEngine;

namespace FarmSim.Planteables 
{
    /// <class name="TechCreator">
    ///     <summary>
    ///         Creates all the tech objects that were saved in the given section before the data is injected in the Update function.
    ///     </summary>
    /// </class>
    public class TechCreator : MonoBehaviour
    {
        private const string TECH_PREFAB_FOLDER = "Tech/";

        private void Awake()
        {
            if (SectionData.Current.techDatas != null)
            {
                foreach(TechData data in SectionData.Current.techDatas)
                {
                    // get the tech prefab
                    var prefab = Resources.Load(TECH_PREFAB_FOLDER + data.prefabName) as GameObject;

                    if (prefab == null)
                    {
                        Debug.LogError("There is no tech prefab at path: " + TECH_PREFAB_FOLDER + data.prefabName);
                    }

                    var gameObject = Instantiate(prefab);
                    gameObject.transform.position = data.pos;

                    gameObject.GetComponent<ITech>().Data = data;
                }
            }
        }
    }
}

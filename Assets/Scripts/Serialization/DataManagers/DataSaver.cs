using FarmSim.Attributes;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace FarmSim.Serialization
{
    /// <class name="DataSaver">
    ///     <summary>
    ///         Saves all data from any object that implements <see cref="ISavable"/> by running their <see cref="ISavable.Save"/>
    ///     </summary>
    /// </class>
    public class DataSaver : MonoBehaviour
    {
        public bool Saving { get; private set; } = false;

        /// <summary>
        ///     Finds all ISaveables in the section/scene and Saves their data to a specific section file.
        /// </summary>
        public void SaveSectionVoid(bool isSavableSection, int sectionNum)
        {
            Saving = true;

            IEnumerable saveables = FindObjectsOfType<MonoBehaviour>().OfType<ISavable>();

            // save every item
            foreach (ISavable s in saveables)
            {
                SavableAttribute attribute = (SavableAttribute)Attribute.GetCustomAttribute(s.GetType(), typeof(SavableAttribute));

                if (attribute == null)
                {
                    Debug.LogError($"No SavableAttribute was found on instance of class {s.GetType()}");
                }

                if (isSavableSection || attribute.GetCanSaveOnAnySection())
                {
                    s.Save();
                }
            }

            PlayerData.Current.SectionNum = sectionNum;

            Debug.Log("plant section count: " + SectionData.Current.plantDatas.Count);
            Debug.Log("dirt section count: " + SectionData.Current.dirtDatas.Count);
            Debug.Log("item num: " + PlayerData.Current.itemDatas.Count);

            // translate it through binary formatter
            if (SerializationManager.Save(SectionData.Current, "Section_" + sectionNum) && SerializationManager.Save(PlayerData.Current, "Player"))
            {
                Debug.Log("Save was succesful");
            }

            Saving = false;
        }
    }
}

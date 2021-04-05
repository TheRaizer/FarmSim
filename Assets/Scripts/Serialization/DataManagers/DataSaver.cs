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
        ///     Finds all ISaveables in the scene and Saves their data.
        /// </summary>
        public IEnumerator SaveAllCo()
        {
            if (!Saving)
            {
                Saving = true;

                IEnumerable saveables = FindObjectsOfType<MonoBehaviour>().OfType<ISavable>();

                // save every item
                foreach (ISavable s in saveables)
                {
                    yield return null;
                    s.Save();
                }

                Debug.Log("plant section count: " + SaveData.Current.plantDatas.Count);
                Debug.Log("dirt section count: " + SaveData.Current.dirtDatas.Count);
                Debug.Log("item num: " + SaveData.Current.playerData.itemDatas.Count);

                // translate it through binary formatter
                if (SerializationManager.Save(SaveData.Current))
                {
                    Debug.Log("Save was succesful");
                }

                Saving = false;
            }
        }

        /// <summary>
        ///     Finds all ISaveables in the scene and Saves their data.
        /// </summary>
        public void SaveAllVoid(bool isSavableSection, int sectionNum, string save="Save")
        {
            Saving = true;

            IEnumerable saveables = FindObjectsOfType<MonoBehaviour>().OfType<ISavable>();

            // save every item
            foreach (ISavable s in saveables)
            {
                Debug.Log(s.GetType());
                SavableAttribute attribute = (SavableAttribute)Attribute.GetCustomAttribute(s.GetType(), typeof(SavableAttribute));

                if(attribute == null)
                {
                    Debug.LogError($"No SavableAttribute was found on instance of class {s.GetType()}");
                }

                if (isSavableSection || attribute.GetCanSaveOnAnySection())
                {
                    s.Save();
                }
            }

            SaveData.Current.SectionNum = sectionNum;

            Debug.Log("plant section count: " + SaveData.Current.plantDatas.Count);
            Debug.Log("dirt section count: " + SaveData.Current.dirtDatas.Count);
            Debug.Log("item num: " + SaveData.Current.playerData.itemDatas.Count);

            // translate it through binary formatter
            if (SerializationManager.Save(SaveData.Current, save))
            {
                Debug.Log("Save was succesful");
            }

            Saving = false;
        }
    }
}

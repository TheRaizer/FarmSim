using FarmSim.Attributes;
using System;
using System.Collections;
using System.IO;
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
        public void SaveSection(bool isSavableSection, int sectionNum)
        {
            Saving = true;

            // overwrite the sections internal day to the current day
            SectionData.Current.InternalDay = TimeData.Current.day;

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

            // translate it through binary formatter
            if (SerializationManager.Save(SectionData.Current, SavePaths.SECTION_PREFIX + sectionNum, SavePaths.SECTIONS_DIRECTORY))
            {
                Debug.Log("Section Save was Succesful");
            }

            Saving = false;
        }

        public void SavePlayer(int sectionNum)
        {
            PlayerData.Current.SectionNum = sectionNum;
            if (SerializationManager.Save(PlayerData.Current, SavePaths.PLAYER_FILE))
            {
                Debug.Log("Player Save was Succesful");
            }
        }

        public void SaveMain(bool isSavableSection, int sectionNum)
        {
            SaveSection(isSavableSection, sectionNum);
            SavePlayer(sectionNum);
            SaveTime();

            MainSaveData mainSave = new MainSaveData();

            string[] filePaths = Directory.GetFiles(Application.persistentDataPath + "/" + SavePaths.SECTIONS_DIRECTORY + "/");

            foreach (string path in filePaths)
            {
                string fileName = Path.GetFileNameWithoutExtension(path);
                SectionData section = (SectionData)SerializationManager.LoadSave(fileName, SavePaths.SECTIONS_DIRECTORY);

                mainSave.sections.Add(section);
            }
            if (SerializationManager.Save(mainSave))
            {
                Debug.Log("Main Save was succesful");
            }
        }

        public void SaveTime()
        {
            if (SerializationManager.Save(TimeData.Current, SavePaths.TIME_FILE))
            {
                Debug.Log("Time Save was succesful");
            }
        }
    }
}

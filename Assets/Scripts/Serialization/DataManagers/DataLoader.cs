using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FarmSim.Serialization
{
    /// <class name="DataLoader">
    ///     <summary>
    ///         Loads the saved data when before the first frame runs.
    ///         <remarks>
    ///             <para>DATA LOADING SHOULD HAPPEN IN A SCENE BEFORE ANY LOADED DATA IS USED.</para>
    ///         </remarks>
    ///     </summary>
    /// </class>
    public class DataLoader : MonoBehaviour
    {
        private void Awake()
        {
            // load player data
            PlayerData.Current = (PlayerData)SerializationManager.LoadSave(SavePaths.PLAYER_FILE);
            TimeData.Current = (TimeData)SerializationManager.LoadSave(SavePaths.TIME_FILE);

            RemoveTemporarySectionData();

            InitializeSectionsData();
        }

        private void RemoveTemporarySectionData()
        {
            // clear all temporary section data that was saved if it exists
            string completeSectionDir = Application.persistentDataPath + "/" + SavePaths.SECTIONS_DIRECTORY;
            if (Directory.Exists(completeSectionDir))
            {
                Directory.Delete(completeSectionDir, true);
            }
        }

        private void InitializeSectionsData()
        {
            // load the main save
            MainSaveData mainSave = (MainSaveData)SerializationManager.LoadSave(SavePaths.MAIN_SAVE_FILE);

            if (mainSave != null)
            {
                foreach (SectionData sd in mainSave.sections)
                {
                    // Create new temp section data with the sections that were manually saved.
                    SerializationManager.Save(sd, SavePaths.SECTION_PREFIX + sd.SectionNum, SavePaths.SECTIONS_DIRECTORY);

                    if (sd.SectionNum == PlayerData.Current.SectionNum)
                    {
                        // assign the current section to be section the player manually saved in
                        SectionData.Current = sd;
                    }
                }
            }

            if (SectionData.Current == null)
            {
                SectionData.Current = new SectionData();
            }
        }

        //TEST CODE
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // scene numbers should corrospond to section numbers + 1
                SceneManager.LoadScene(PlayerData.Current.SectionNum + 1);
            }
        }
    }
}

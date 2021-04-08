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

            MainSaveData mainSave = (MainSaveData)SerializationManager.LoadSave(SavePaths.MAIN_SAVE_FILE);

            // load the section data that the player last manually saved in
            SectionData.Current = mainSave?.sections.Find(x => x.SectionNum == PlayerData.Current.SectionNum);

            if(SectionData.Current == null)
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

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
            PlayerData.Current = (PlayerData)SerializationManager.Load(Application.persistentDataPath + "/saves/Player.save");

            // load the section data that the player was last in
            SectionData.Current = (SectionData)SerializationManager.Load(Application.persistentDataPath + "/saves/Section_" + PlayerData.Current.SectionNum + ".save");
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

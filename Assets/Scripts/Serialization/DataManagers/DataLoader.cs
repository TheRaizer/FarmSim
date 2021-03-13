using System.Collections;
using System.Linq;
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
            // this can be changed to happen when a button to load is pressed instead.
            SaveData.Current = (SaveData)SerializationManager.Load(Application.persistentDataPath + "/saves/Save.save");
        }

        //TEST CODE
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(1);
            }
        }
    }
}

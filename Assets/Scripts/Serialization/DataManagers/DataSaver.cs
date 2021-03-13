using FarmSim.Serialization;
using System.Collections;
using System.Linq;
using UnityEngine;

public class DataSaver : MonoBehaviour
{
    public bool Saving { get; private set; } = false;

    /// <summary>
    ///     Finds all ISaveables in the scene and Saves their data.
    /// </summary>
    public IEnumerator SaveAll()
    {
        if (!Saving)
        {
            Saving = true;
            IEnumerable saveables = FindObjectsOfType<MonoBehaviour>().OfType<ISavable>();

            foreach (ISavable s in saveables)
            {
                yield return null;
                s.Save();
            }

            if (SerializationManager.Save(SaveData.Current))
            {
                Debug.Log("Save was succesful");
            }

            Saving = false;
        }
    }
}

using FarmSim.Serialization;
using System.Collections;
using System.Linq;
using UnityEngine;

public class DataSaver : MonoBehaviour
{
    public bool Saved { get; private set; } = false;

    private bool saving = false;

    /// <summary>
    ///     Finds all ISaveables in the scene and Saves their data.
    /// </summary>
    public IEnumerator SaveAll()
    {
        if (!saving)
        {
            saving = true;
            IEnumerable saveables = FindObjectsOfType<MonoBehaviour>().OfType<ISaveable>();

            foreach (ISaveable s in saveables)
            {
                yield return null;
                s.Save();
            }

            if (SerializationManager.Save(SaveData.current))
            {
                Debug.Log("Save was succesful");
            }

            saving = false;
        }
        Saved = true;
    }
}

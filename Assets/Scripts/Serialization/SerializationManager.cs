using FarmSim.SavableData;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace FarmSim.Serialization
{
    /// <class name="SerializationManager">
    ///     <summary>
    ///         Manages Serialization of Game data using a Binary Formatter.
    ///     </summary>
    /// </class>
    public class SerializationManager
    {
        /// <summary>
        ///     Saves a given object to a certain file name.
        /// </summary>
        /// <param name="saveData">The serializable object to save.</param>
        /// <param name="saveName">The name of the file.</param>
        /// <returns></returns>
        public static bool Save(object saveData, string saveName = SavePaths.MAIN_SAVE_FILE, string directory = SavePaths.GENERAL_DIRECTORY)
        {
            BinaryFormatter formatter = GetBinaryFormatter();

            string directoryPath = Path.Combine(Application.persistentDataPath + "/", directory);
            string savePath = Path.Combine(Application.persistentDataPath + "/" + directory + "/", saveName + ".save");

            if (!Directory.Exists(directoryPath))
            {
                // create a directory if none exist
                Directory.CreateDirectory(directoryPath);
            }

            FileStream file = File.Create(savePath);

            formatter.Serialize(file, saveData);
            file.Close();

            return true;
        }

        /// <summary>
        ///     Loads some object of Data given a file path.
        /// </summary>
        /// <param name="path">The file path.</param>
        /// <returns><see cref="object"/> containing data.</returns>
        public static object LoadSave(string saveName, string directory = "General")
        {
            string savePath = Path.Combine(Application.persistentDataPath + "/" + directory + "/", saveName + ".save");

            if (!File.Exists(savePath))
            {
                Debug.LogWarning($"No file exists at {savePath}");
                return null;
            }

            BinaryFormatter formatter = GetBinaryFormatter();

            FileStream file = File.Open(savePath, FileMode.Open);

            try
            {
                object save = formatter.Deserialize(file);
                file.Close();
                return save;
            }
            catch
            {
                Debug.LogErrorFormat("Failed to load file at {0}", savePath);
                file.Close();
                return null;
            }
        }

        public static BinaryFormatter GetBinaryFormatter()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            SurrogateSelector selector = new SurrogateSelector();

            Vector2SerializationSurrogate vector2Surrogate = new Vector2SerializationSurrogate();

            selector.AddSurrogate(typeof(Vector2), new StreamingContext(StreamingContextStates.All), vector2Surrogate);

            formatter.SurrogateSelector = selector;

            return formatter;
        }
    }
}
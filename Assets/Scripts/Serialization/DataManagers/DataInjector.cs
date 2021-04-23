using System.Collections;
using System.Linq;
using UnityEngine;

namespace FarmSim.Serialization
{
    /// <class name="DataInjector">
    ///     <summary>
    ///         Runs <see cref="ILoadable.Load"/> on every <see cref="ILoadable"/> in the current scene.
    ///     </summary>
    /// </class>
    public class DataInjector : MonoBehaviour
    {
        private bool loading = false;

        /// <summary>
        ///     Finds all ILoadables in the scene and injects the loaded data into them.
        /// </summary>
        public IEnumerator LoadAll()
        {
            if (!loading)
            {
                loading = true;
                IEnumerable loadables = FindObjectsOfType<MonoBehaviour>().OfType<ILoadable>();

                foreach (ILoadable l in loadables)
                {
                    yield return null;
                    l.Load();
                }

                loading = false;
            }
            Debug.Log("Succesfully loaded");
        }

        /// <summary>
        ///     Finds all ILoadables in the scene and injects the loaded data into them.
        /// </summary>
        public void LoadAllVoid()
        {
            if (!loading)
            {
                loading = true;
                IEnumerable loadables = FindObjectsOfType<MonoBehaviour>().OfType<ILoadable>();

                foreach (ILoadable l in loadables)
                {
                    l.Load();
                }

                loading = false;
            }
            Debug.Log("Succesfully loaded");
        }
    }
}

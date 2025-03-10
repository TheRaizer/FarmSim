﻿using System.Collections;
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
        /// <summary>
        ///     Number of ILoadables to load before yielding the coroutine.
        /// </summary>
        [SerializeField] private int loadInterval = 5;
        private bool injecting = false;

        /// <summary>
        ///     Finds all ILoadables in the scene and injects the loaded data into them.
        /// </summary>
        public IEnumerator InjectAllData()
        {
            if (!injecting)
            {
                int i = 0;
                injecting = true;
                IEnumerable loadables = FindObjectsOfType<MonoBehaviour>().OfType<ILoadable>();

                foreach (ILoadable l in loadables)
                {
                    i++;

                    // only when it is on the interval do we yield to allow other work to be done
                    if (i % loadInterval == 0)
                        yield return null;
                    l.Load();
                }

                injecting = false;
            }
            Debug.Log("Succesfully injected data");
        }

        public IEnumerator PostInjectionAll()
        {
            if (!injecting)
            {
                int i = 0;
                injecting = true;
                IEnumerable postLoads = FindObjectsOfType<MonoBehaviour>().OfType<IOccurPostLoad>();
                foreach (IOccurPostLoad p in postLoads)
                {
                    i++;

                    // only when it is on the interval do we yield to allow other work to be done
                    if (i % loadInterval == 0)
                        yield return null;
                    p.PostLoad();
                }
            }
            injecting = false;
        }
    }
}

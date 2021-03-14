using UnityEngine;

namespace FarmSim.Loading
{
    /// <class name="OccurPostLoad">
    ///     <summary>
    ///         Class that has the logic to do something after the loading of all the data has occured.
    ///     </summary>
    /// </class>
    public abstract class OccurPostLoad : MonoBehaviour
    {
        private LoadingOrder loadingOrder = null;
        private bool initialized = false;

        protected virtual void Awake()
        {
            loadingOrder = FindObjectOfType<LoadingOrder>();
        }
        protected virtual void Update()
        {
            // if we've loaded everything and we haven't run PostLoad()
            if (loadingOrder.LoadedAll && !initialized)
            {
                // execute something after loading
                PostLoad();
                initialized = true;
            }
        }

        protected abstract void PostLoad();
    }
}

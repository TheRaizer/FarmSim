using FarmSim.Grid;
using FarmSim.Serialization;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace FarmSim.Loading
{
    /// <class name="LoadingOrder">
    ///     <summary>
    ///         Manages the loading order of Data. Starting by loading the grid section, then the data of each node.
    ///     </summary>
    /// </class>
    public class LoadingOrder : MonoBehaviour
    {
        [SerializeField] private GameObject introScreenCover;
        private DataInjector dataInjector;
        private NodeGrid nodeGrid;

        public bool LoadedAll { get; private set; } = false;

        private void Awake()
        {
            dataInjector = GetComponent<DataInjector>();
            nodeGrid = FindObjectOfType<NodeGrid>();
        }

        void Update()
        {
            if (nodeGrid.LoadedSection && !LoadedAll)
            {
                // once we've loaded the grid load the rest of the data
                dataInjector.LoadAllVoid();
                PostLoadAll();

                LoadedAll = true;
            }

            if (LoadedAll)
            {
                // once finished loading close the screen cover
                introScreenCover.SetActive(false);
            }
        }

        private void PostLoadAll()
        {
            IEnumerable postLoads = FindObjectsOfType<MonoBehaviour>().OfType<IOccurPostLoad>();
            foreach (IOccurPostLoad p in postLoads)
            {
                p.PostLoad();
            }
        }
    }
}

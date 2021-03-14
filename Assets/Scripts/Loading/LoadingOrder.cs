using FarmSim.Grid;
using FarmSim.Serialization;
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
        private NodeGrid nodeGrid;
        private DataInjector dataInjector;

        public bool LoadedAll { get; private set; } = false;

        private void Awake()
        {
            nodeGrid = GetComponent<NodeGrid>();
            dataInjector = GetComponent<DataInjector>();
        }

        void Update()
        {
            if (nodeGrid.LoadedSection && !LoadedAll)
            {
                // once we've loaded the grid load the rest of the data
                dataInjector.LoadAllVoid();
                LoadedAll = true;
            }

            if (LoadedAll)
            {
                // once finished loading close the screen cover
                introScreenCover.SetActive(false);
            }
        }
    }
}

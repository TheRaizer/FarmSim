using FarmSim.Grid;
using FarmSim.Serialization;
using System.Collections;
using UnityEngine;

namespace FarmSim.Loading
{
    /// <class name="LoadingOrder">
    ///     <summary>
    ///         Manages the loading order of Data. Starting by loading the grid section, then the data of each node, then any post loading.
    ///     </summary>
    /// </class>
    public class LoadingOrder : MonoBehaviour
    {
        [SerializeField] private GameObject introScreenCover;

        private DataInjector dataInjector;
        private NodeGrid nodeGrid;

        public bool LoadedAll { get; private set; } = false;
        private bool startedLoading = false;

        private void Awake()
        {
            dataInjector = GetComponent<DataInjector>();
            nodeGrid = FindObjectOfType<NodeGrid>();
        }

        void Update()
        {
            if (nodeGrid.LoadedSection && !LoadedAll && !startedLoading)
            {
                startedLoading = true;
                // once we've loaded the grid load the rest of the data
                StartCoroutine(InjectCo());
            }

            if (LoadedAll)
            {
                // once finished loading close the screen cover
                introScreenCover.SetActive(false);
            }
        }

        private IEnumerator InjectCo()
        {
            yield return StartCoroutine(dataInjector.InjectAllData());
            yield return StartCoroutine(dataInjector.PostInjectionAll());

            LoadedAll = true;
        }
    }
}

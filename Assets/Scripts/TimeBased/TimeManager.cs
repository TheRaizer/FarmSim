using FarmSim.Grid;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FarmSim.TimeBased
{
    public class TimeManager : MonoBehaviour
    {
        [SerializeField] private GameObject dayPassBackground;

        private List<ITimeBased> timeBasedObjects = null;
        private NodeGrid grid;
        private DataSaver dataSaver;

        public int CurrentDay { get; private set; } = 0;

        private void Awake()
        {
            grid = GetComponent<NodeGrid>();
            dataSaver = GetComponent<DataSaver>();
        }

        private void Update()
        {
            if (grid.LoadedSection && timeBasedObjects == null)
            {
                timeBasedObjects = FindObjectsOfType<MonoBehaviour>().OfType<ITimeBased>().ToList();
            }
            if(dataSaver.Saved && dayPassBackground.activeSelf)
            {
                dayPassBackground.SetActive(false);
            }
        }

        private void LateUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                MoveToNextDay();
            }
        }

        private void MoveToNextDay()
        {
            CurrentDay++;
            timeBasedObjects.ForEach(timeBased => timeBased.OnDayPass());

            dayPassBackground.SetActive(true);

            StartCoroutine(dataSaver.SaveAll());
        }
    }
}
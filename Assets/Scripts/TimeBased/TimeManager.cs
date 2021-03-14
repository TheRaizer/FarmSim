using FarmSim.Grid;
using FarmSim.Serialization;
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
            if (!dataSaver.Saving && dayPassBackground.activeInHierarchy)
            {
                dayPassBackground.SetActive(false);
            }
            else if(dataSaver.Saving && !dayPassBackground.activeInHierarchy)
            {
                dayPassBackground.SetActive(true);
            }
        }

        private void LateUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Space) && !dataSaver.Saving)
            {
                MoveToNextDay();
            }
        }

        private void MoveToNextDay()
        {
            dayPassBackground.SetActive(true);
            CurrentDay++;
            timeBasedObjects.ForEach(timeBased => timeBased.OnDayPass());

            StartCoroutine(dataSaver.SaveAll());
        }
    }
}
using FarmSim.Grid;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FarmSim.TimeBased
{
    public class TimeManager : MonoBehaviour
    {
        private List<ITimeBased> timeBasedObjects = null;
        private NodeGrid grid;
        public int CurrentDay { get; private set; } = 0;

        private void Awake()
        {
            grid = GetComponent<NodeGrid>();
        }

        private void Update()
        {
            if (grid.LoadedSection && timeBasedObjects == null)
            {
                timeBasedObjects = FindObjectsOfType<MonoBehaviour>().OfType<ITimeBased>().ToList();
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
        }
    }
}
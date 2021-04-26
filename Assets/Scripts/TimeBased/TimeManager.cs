using FarmSim.Grid;
using FarmSim.Serialization;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FarmSim.TimeBased
{
    /// <class name="TimeManager">
    ///     <summary>
    ///         Manages in-game time as well as running the DayPass() function on each ITimeBased GameObjects.
    ///     </summary>
    /// </class>
    public class TimeManager : MonoBehaviour
    {
        [SerializeField] private GameObject dayPassBackground;

        private DataSaver dataSaver;
        private NodeGrid nodeGrid;

        private void Awake()
        {
            nodeGrid = FindObjectOfType<NodeGrid>();
            dataSaver = GetComponent<DataSaver>();
        }

        private void Update()
        {
            if (!dataSaver.Saving && dayPassBackground.activeInHierarchy)
            {
                dayPassBackground.SetActive(false);
            }
            else if (dataSaver.Saving && !dayPassBackground.activeInHierarchy)
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

            // get all time based objects
            List<ITimeBased> timeBasedObjects = FindObjectsOfType<MonoBehaviour>().OfType<ITimeBased>().ToList();

            TimeData.Current.day++;

            // pass the time on each object by 1 day
            timeBasedObjects.ForEach(timeBased => timeBased.OnTimePass());

            // execute the main save with the current section as the origin
            dataSaver.SaveMain(nodeGrid.IsSavableSection, nodeGrid.SectionNum);
        }
    }
}
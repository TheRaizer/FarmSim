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

        private List<ITimeBased> timeBasedObjects = null;
        private DataSaver dataSaver;

        private void Awake()
        {
            dataSaver = GetComponent<DataSaver>();
        }

        private void Update()
        {
            if (NodeGrid.Instance.LoadedSection && timeBasedObjects == null)
            {
                timeBasedObjects = FindObjectsOfType<MonoBehaviour>().OfType<ITimeBased>().ToList();
            }
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
            TimeData.Current.day++;
            timeBasedObjects.ForEach(timeBased => timeBased.OnDayPass());

            dataSaver.SaveMain(NodeGrid.Instance.IsSavableSection, NodeGrid.Instance.SectionNum);
        }
    }
}
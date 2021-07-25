using FarmSim.SavableData;
using FarmSim.Serialization;
using UnityEngine;

namespace FarmSim.TimeBased
{
    public class DayClock : MonoBehaviour, ILoadable
    {
        [SerializeField] private float realSecondsForTenGameMin = 5;

        // start day at 7am
        private const int INIT_MINS = 420;
        // automatically end day at 3am
        private const int END_DAY_MINS = 1620;

        private int minsPassed = INIT_MINS;

        private int HoursPassed => Mathf.FloorToInt(minsPassed / 60);
        private TimeManager timeManager;
        private float lastTime = 0;


        private void Awake()
        {
            timeManager = GetComponent<TimeManager>();
        }

        private void Update()
        {
            if (Time.time - lastTime >= realSecondsForTenGameMin)
            {
                UpdateClock();
                lastTime = Time.time;
            }
        }

        private void UpdateClock()
        {
            minsPassed += 10;

            // automatically end the day when the time passes 3am
            if (minsPassed > END_DAY_MINS)
            {
                minsPassed = INIT_MINS;
                timeManager.MoveToNextDay();
            }

            TimeData.Current.minsPassed = minsPassed;
            Debug.Log(Get12HourTime());
        }

        private string Get12HourTime()
        {
            // if the mins passed is between 12pm and 12am then time is 'pm' else 'am'
            string suffix = HoursPassed >= 12 && HoursPassed < 24 ? "PM" : "AM";
            string hour;

            if(HoursPassed % 12 != 0)
            {
                hour = (HoursPassed % 12).ToString();
            }
            else
            {
                hour = "12";
            }

            string min = (minsPassed % 60) == 0 ? "00" : (minsPassed % 60).ToString();

            return hour + ":" + min + suffix;
        }

        public void Load()
        {
            minsPassed = TimeData.Current.minsPassed;
            if(minsPassed == 0)
            {
                minsPassed = INIT_MINS;
            }
        }
    }
}
using FarmSim.Loading;
using FarmSim.Serialization;
using UnityEngine;

namespace FarmSim.TimeBased
{
    public class TimeCatchUp : MonoBehaviour, ITimeBased, IOccurPostLoad
    {
        public virtual void OnTimePass(int daysPassed = 1) { }

        /// <summary>
        ///     Catches up on the time difference between a sections internal day and the global day.
        ///     
        ///     <para>
        ///         Should be called at the end of Load function.
        ///     </para>
        /// </summary>
        public void CatchupOnTime()
        {
            if (SectionData.Current.internalDay < TimeData.Current.day)
            {
                // catchup on the time difference between global and section internal days
                int timeDiff = TimeData.Current.day - SectionData.Current.internalDay;
                OnTimePass(timeDiff);
            }
        }

        public virtual void PostLoad()
        {
            CatchupOnTime();
        }
    }
}

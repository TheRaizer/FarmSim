using FarmSim.TimeBased;
using UnityEngine;

namespace FarmSim.Planteables
{

    /// <class name="Planteable">
    ///     <summary>
    ///         Base class for any planteable gameObject.
    ///     </summary>
    /// </class>
    public class Planteable : MonoBehaviour, ITimeBased
    {
        [SerializeField] private int daysToGrow = 0;

        public void OnDayPass()
        {
        }
    }
}
using UnityEngine;

namespace FarmSim.Entity
{
    /// <class name="AmountManager">
    ///     <summary>
    ///         Manages some <see cref="int"/> amount with basic functionality.
    ///     </summary>
    /// </class>
    public class AmountManager : MonoBehaviour
    {
        [SerializeField] protected int maxAmt;
        public int CurrentAmt { get; private set; }

        public void IncreaseMax(int newMax) => maxAmt = newMax;
        public void IncreaseAmt(int amt) => CurrentAmt = Mathf.Clamp(CurrentAmt + amt, 0, maxAmt);
        public void DecreaseAmt(int amt) => CurrentAmt = Mathf.Clamp(CurrentAmt - amt, 0, maxAmt);
        public void ZeroOut() => CurrentAmt = 0;
        public void MaxOut() => CurrentAmt = maxAmt;
    }
}
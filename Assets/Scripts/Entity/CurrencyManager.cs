using TMPro;
using UnityEngine;

namespace FarmSim.Entity
{
    /// <class name="CurrencyManager">
    ///     <summary>
    ///         Manages specifically the amount of currency an entity has.
    ///     </summary>
    /// </class>
    public class CurrencyManager : AmountManager
    {
        [SerializeField] private TextMeshProUGUI amtText;

        private void Awake()
        {
            onAmtChange = () => amtText.SetText(CurrentAmt.ToString());
        }

        public bool Subtractable(int amt)
        {
            return CurrentAmt - amt >= 0;
        }
    }
}

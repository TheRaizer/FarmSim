using FarmSim.Attributes;
using FarmSim.Entity;
using FarmSim.SavableData;
using FarmSim.Serialization;
using TMPro;
using UnityEngine;

namespace FarmSim.Player
{
    /// <class name="PlayerCurrency">
    ///     <summary>
    ///         Manages the currency of the player.
    ///     </summary>
    /// </class>
    [Savable(true)]
    public class PlayerCurrency : AmountManager, ISavable, ILoadable
    {
        [SerializeField] private TextMeshProUGUI amtText;

        private void Awake()
        {
            if (amtText != null)
                onAmtChange = () => amtText.SetText(CurrentAmt.ToString());
            else
                Debug.LogWarning("Amt text is not assigned in PlayerCurrency.cs");
        }

        public bool Subtractable(int amt)
        {
            return CurrentAmt - amt >= 0;
        }

        public void Save()
        {
            PlayerData.Current.currency = CurrentAmt;
        }

        public void Load()
        {
            ZeroOut();
            IncreaseAmt(PlayerData.Current.currency);
        }
    }
}

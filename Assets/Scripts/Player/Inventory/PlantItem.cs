using FarmSim.Enums;
using UnityEngine;


namespace FarmSim.Player
{
    [System.Serializable]
    public class PlantItem
    {
        [field: SerializeField] public PlantTypes PlantType { get; private set; }
        [field: SerializeField] public int Price { get; private set; }
        [field: SerializeField] public int Amt { get; private set; } = 0;

        public void AddToAmt(int amt)
        {
            Amt += amt;
        }
    }
}

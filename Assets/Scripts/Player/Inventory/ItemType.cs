using UnityEngine;
using UnityEngine.UI;

namespace FarmSim.Player
{

    [CreateAssetMenu]
    public class ItemType : ScriptableObject
    {
        [field: SerializeField] public string ItemName { get; private set; }
        [field: SerializeField] public int Price { get; private set; }
        [field: SerializeField] public int MaxCarryAmt { get; private set; }
        [field: SerializeField] public GameObject IconPrefab { get; private set; }
    }
}

using UnityEngine;

namespace FarmSim.Player
{

    [CreateAssetMenu]
    public class ItemType : ScriptableObject
    {
        [field: SerializeField] public string ItemName { get; private set; }
        [field: SerializeField] public int Price { get; private set; }
    }
}

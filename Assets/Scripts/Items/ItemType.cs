using UnityEngine;

namespace FarmSim.Items
{
    /// <class name="ItemType">
    ///     <summary>
    ///         A SO that manages the parts that make up the general components of an item.
    ///     </summary>
    /// </class>
    [CreateAssetMenu]
    public class ItemType : ScriptableObject
    {
        [field: SerializeField] public string ItemName { get; private set; }
        [field: SerializeField] public int Price { get; private set; }
        [field: SerializeField] public int MaxCarryAmt { get; private set; }
        [field: SerializeField] public GameObject IconPrefab { get; private set; }
        [field: SerializeField] public GameObject ShopIconPrefab { get; private set; }
        [SerializeField] private GameObject worldItemPrefab;

        public void SpawnWorldItem(Vector2 spawnPos, int amt)
        {
            var gameObject = Instantiate(worldItemPrefab);

            gameObject.transform.position = spawnPos;

            var rb = gameObject.GetComponent<Rigidbody2D>();

            WorldItem item = gameObject.GetComponent<WorldItem>();

            item.Data = new WorldItemData(amt, spawnPos, worldItemPrefab.name);

            Vector2 direction = Random.insideUnitCircle.normalized;
            rb.AddForce(direction * 5, ForceMode2D.Impulse);
        }

        public override string ToString()
        {
            return ItemName + " || Costs: " + Price + " || Max amount: " + MaxCarryAmt;
        }
    }
}

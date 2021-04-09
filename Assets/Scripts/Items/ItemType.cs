using UnityEngine;

namespace FarmSim.Items
{

    [CreateAssetMenu]
    public class ItemType : ScriptableObject
    {
        [field: SerializeField] public string ItemName { get; private set; }
        [field: SerializeField] public int Price { get; private set; }
        [field: SerializeField] public int MaxCarryAmt { get; private set; }
        [field: SerializeField] public GameObject IconPrefab { get; private set; }
        [field: SerializeField] public GameObject ShopIconPrefab { get; private set; }
        [field: SerializeField] public GameObject WorldItemPrefab { get; private set; }

        public void SpawnWorldItem(Vector2 spawnPos, int amt)
        {
            var gameObject = Instantiate(WorldItemPrefab);

            gameObject.transform.position = spawnPos;

            var rb = gameObject.GetComponent<Rigidbody2D>();

            WorldItem item = gameObject.GetComponent<WorldItem>();
            item.Amt = amt;

            Vector2 direction = Random.insideUnitCircle.normalized;
            rb.AddForce(direction * 5, ForceMode2D.Impulse);
        }

        public override string ToString()
        {
            return ItemName + " || Costs: " + Price + " || Max amount: " + MaxCarryAmt;
        }
    }
}

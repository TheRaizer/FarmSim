using UnityEngine;

namespace FarmSim.Items
{
    public class AddToInventoryOnClick : MonoBehaviour
    {
        [SerializeField] private GameObject worldItem;
        [SerializeField] private int amt;

        private void OnMouseOver()
        {
            if (Input.GetMouseButtonDown(1))
            {
                var gameObject = Instantiate(worldItem);

                gameObject.transform.position = transform.position;

                var rb = gameObject.GetComponent<Rigidbody2D>();

                WorldItem item = gameObject.GetComponent<WorldItem>();
                item.Amt = amt;

                Vector2 direction = Random.insideUnitCircle.normalized;
                rb.AddForce(direction * 2, ForceMode2D.Impulse);
            }
        }
    }
}
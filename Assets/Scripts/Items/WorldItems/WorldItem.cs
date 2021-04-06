﻿using System.Collections;
using UnityEngine;

namespace FarmSim.Items
{
    /// <class name="SlotsHandler">
    ///     <summary>
    ///         Manages a spawned world item. 
    ///     </summary>
    /// </class>
    public class WorldItem : MonoBehaviour
    {
        [SerializeField] private ItemType itemType;

        public int Amt { private get; set; } = 4;

        private Inventory inventory;
        private Transform player;

        private bool moveToPlayer = false;

        private readonly WaitForSeconds followTime = new WaitForSeconds(2);
        private readonly WaitForSeconds waitTime = new WaitForSeconds(0.5f);
        private const float speed = 3;

        private void Awake()
        {
            inventory = FindObjectOfType<Inventory>();
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        private void Update()
        {
            if (moveToPlayer)
            {
                MoveToPlayer();
            }
        }

        private void MoveToPlayer()
        {
            Vector2 newPos = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
            Vector2 distance = (Vector2)player.position - newPos;

            if (AddToInventoryInRange(distance))
                return;

            transform.position = newPos;
        }

        private bool AddToInventoryInRange(Vector2 distance)
        {
            if (Mathf.Abs(distance.x) < 0.001 || Mathf.Abs(distance.y) < 0.001)
            {
                inventory.AddToInventory(itemType, Amt, () => Destroy(gameObject), () => moveToPlayer = false);
                return true;
            }
            return false;
        }

        private IEnumerator WaitCo()
        {
            yield return waitTime;
            StartCoroutine(AddToInventoryCo());
        }

        private IEnumerator AddToInventoryCo()
        {
            moveToPlayer = true;

            yield return followTime;

            inventory.AddToInventory(itemType, Amt, () => Destroy(gameObject));
            moveToPlayer = false;
        }

        public void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                if (!moveToPlayer)
                {
                    StartCoroutine(WaitCo());
                }
            }
        }
    }
}

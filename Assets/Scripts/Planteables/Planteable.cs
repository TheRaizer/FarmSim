using FarmSim.Enums;
using System.Collections.Generic;
using UnityEngine;

namespace FarmSim.Planteables
{

    /// <class name="Planteable">
    ///     <summary>
    ///         Base class for any planteable gameObject.
    ///     </summary>
    /// </class>
    public class Planteable : MonoBehaviour
    {
        [SerializeField] private int daysToGrow = 0;
        [SerializeField] private List<Sprite> spriteLifeCycle;
        [SerializeField] private PlantTypes plantType;
        [SerializeField] private int maxAmtToDrop = 0;
        [SerializeField] private int minAmtToDrop = 0;

        private SpriteRenderer spriteRenderer;

        private int spriteChangeInterval = 0;
        private int currentGrowthDay = 1;
        private int spriteIdx = 1;

        public bool CanHarvest { get; private set; }

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = spriteLifeCycle[0];
            spriteChangeInterval = Mathf.CeilToInt((float)daysToGrow / spriteLifeCycle.Count);
        }

        public void Grow()
        {
            if (currentGrowthDay > daysToGrow)
                return;
            CheckSpriteChange();
            currentGrowthDay++;
        }

        public void OnHarvest()
        {
            int amtToDrop = Random.Range(minAmtToDrop, maxAmtToDrop);
            PlayerInventory inventory = FindObjectOfType<PlayerInventory>();
            inventory.AddToInventory(plantType, amtToDrop);
            Destroy(gameObject);
        }

        private void CheckSpriteChange()
        {
            // if its on the interval to change and the sprite index isnt the last sprite
            if (currentGrowthDay == spriteChangeInterval * spriteIdx && spriteIdx != spriteLifeCycle.Count - 1)
            {
                spriteRenderer.sprite = spriteLifeCycle[spriteIdx];
                spriteIdx++;
            }
            // if the current growth day is the last day thats when we can assign the last sprite
            else if (currentGrowthDay == daysToGrow)
            {
                spriteRenderer.sprite = spriteLifeCycle[spriteLifeCycle.Count - 1];
                CanHarvest = true;
            }
        }
    }
}
using FarmSim.Player;
using System.Collections.Generic;
using UnityEngine;
using FarmSim.TimeBased;

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

        [SerializeField] private int maxAmtToDrop = 0;
        [SerializeField] private int minAmtToDrop = 0;

        /// <summary>
        ///     List of sprites that show the plants growth.
        /// </summary>
        [SerializeField] private List<Sprite> spriteLifeCycle;
        [SerializeField] private ItemType itemType;

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

        /// <summary>
        ///     Function that grows the given planteable and should be called in some class that has a function <see cref="ITimeBased.OnDayPass"/>.
        /// </summary>
        public void Grow()
        {
            if (currentGrowthDay > daysToGrow)
                return;
            CheckSpriteChange();
            currentGrowthDay++;
        }

        /// <summary>
        ///     Adds to the players inventory an amount within a 
        ///     given range and destroys the Planteable gameObject.
        /// </summary>
        public void OnHarvest()
        {
            int amtToDrop = Random.Range(minAmtToDrop, maxAmtToDrop);

            PlayerInventory inventory = FindObjectOfType<PlayerInventory>();
            inventory.AddToInventory(itemType, amtToDrop);

            Destroy(gameObject);
        }

        /// <summary>
        ///     Check to see if the sprite must change on the passage of a day.
        /// </summary>
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
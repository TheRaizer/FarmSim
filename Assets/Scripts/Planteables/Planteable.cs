using FarmSim.TimeBased;
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

        private SpriteRenderer spriteRenderer;

        private int spriteChangeInterval = 0;
        private int currentGrowthDay = 1;
        private int spriteIdx = 1;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = spriteLifeCycle[0];
            spriteChangeInterval = Mathf.CeilToInt((float)daysToGrow / spriteLifeCycle.Count);
            Debug.Log(spriteChangeInterval);
        }

        public void Grow()
        {
            if (currentGrowthDay > daysToGrow)
                return;
            CheckSpriteChange();
            currentGrowthDay++;
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
            }
        }
    }
}
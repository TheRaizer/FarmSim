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
        private int currentGrowthDay = 0;
        private int spriteIdx = 0;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = spriteLifeCycle[spriteIdx];
            spriteChangeInterval = Mathf.CeilToInt(daysToGrow / spriteLifeCycle.Count);
        }

        public void Grow()
        {
            if (currentGrowthDay >= daysToGrow)
                return;
            currentGrowthDay++;
            CheckSpriteChange();
        }

        private void CheckSpriteChange()
        {
            if (currentGrowthDay == spriteChangeInterval * spriteIdx)
            {
                spriteIdx++;
                spriteRenderer.sprite = spriteLifeCycle[spriteIdx];
            }
        }
    }
}
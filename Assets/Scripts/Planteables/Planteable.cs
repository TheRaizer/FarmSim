using FarmSim.Attributes;
using FarmSim.Enums;
using FarmSim.Items;
using FarmSim.Loading;
using FarmSim.Serialization;
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
    [Savable(false)]
    public class Planteable : MonoBehaviour, IOccurPostLoad, ISavable
    {
        [field: SerializeField] public ToolTypes ToolToHarvestWith { get; private set; }
        [SerializeField] private string originalPrefabName = null;
        /// <summary>
        ///     Includes the day it was planted.
        /// </summary>
        [SerializeField] private int daysToGrow = 0;

        [SerializeField] private int maxAmtToDrop = 0;
        [SerializeField] private int minAmtToDrop = 0;

        /// <summary>
        ///     List of sprites that show the plants growth.
        /// </summary>
        [SerializeField] private List<Sprite> spriteLifeCycle;
        [SerializeField] private ItemType itemType;

        public bool CanHarvest => Data.CanHarvest;
        public void SetDataId(string id) => Data.Id = id;
        public PlanteableData Data { private get; set; } = new PlanteableData("", 1, 0, false);
        private SpriteRenderer spriteRenderer;

        private int spriteChangeInterval = 0;

        private void Awake()
        {
            Data.PrefabName = originalPrefabName;
            spriteRenderer = GetComponent<SpriteRenderer>();
            spriteChangeInterval = Mathf.CeilToInt((float)daysToGrow / spriteLifeCycle.Count);
        }

        /// <summary>
        ///     Function that grows the given planteable and should be called in some class that has a function <see cref="ITimeBased.OnTimePass"/>.
        /// </summary>
        public void Grow(int daysPassed)
        {
            if (Data.CurrentGrowthDay >= daysToGrow)
                return;
            CheckSpriteChange();
            Data.CurrentGrowthDay += daysPassed;

            // if the current growth day is the last day thats when we can assign the last sprite
            if (Data.CurrentGrowthDay >= daysToGrow)
            {
                Data.CanHarvest = true;
            }
        }

        /// <summary>
        ///     Adds to the players inventory an amount within a 
        ///     given range and destroys the Planteable gameObject.
        /// </summary>
        public void OnHarvest()
        {
            int amtToDrop = Random.Range(minAmtToDrop, maxAmtToDrop + 1);

            for (int i = 0; i < amtToDrop; i++)
            {
                itemType.SpawnWorldItem(transform.position, 1);
            }

            // we do not need to check if it contains the plant because for a plant to be harvested the game must have saved/passed a day at least once.
            SectionData.Current.plantDatas.Remove(Data);
            Destroy(gameObject);
        }

        /// <summary>
        ///     Check to see if the sprite must change on the passage of a day.
        /// </summary>
        private void CheckSpriteChange()
        {
            // if the plant is still growing
            if (Data.CurrentGrowthDay <= daysToGrow)
            {
                // calculate the sprite idx
                Data.SpriteIdx = Mathf.RoundToInt((float)Data.CurrentGrowthDay / spriteChangeInterval);

                // assign the sprite
                spriteRenderer.sprite = spriteLifeCycle[Data.SpriteIdx];
            }
            else
            {
                // assign the last sprite
                spriteRenderer.sprite = spriteLifeCycle[spriteLifeCycle.Count - 1];
            }
        }

        public void Save()
        {
            if (!SectionData.Current.plantDatas.Contains(Data))
            {
                SectionData.Current.plantDatas.Add(Data);
            }
        }

        public void PostLoad()
        {
            spriteRenderer.sprite = spriteLifeCycle[Data.SpriteIdx];
        }
    }
}
﻿using FarmSim.Player;
using System.Collections.Generic;
using UnityEngine;
using FarmSim.TimeBased;
using FarmSim.Serialization;

namespace FarmSim.Planteables
{

    /// <class name="Planteable">
    ///     <summary>
    ///         Base class for any planteable gameObject.
    ///     </summary>
    /// </class>
    public class Planteable : MonoBehaviour, ISavable
    {
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
        public PlanteableData Data { private get; set; } = new PlanteableData("", 1, 1, false);
        private SpriteRenderer spriteRenderer;

        private int spriteChangeInterval = 0;

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
            if (Data.CurrentGrowthDay > daysToGrow)
                return;
            CheckSpriteChange();
            Data.CurrentGrowthDay++;
            Debug.Log("grow");
            Debug.Log("CurrentGrowthDay: " + Data.CurrentGrowthDay);
            Debug.Log("sprite idx: " + Data.SpriteIdx);
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
            if (Data.CurrentGrowthDay == spriteChangeInterval * Data.SpriteIdx && Data.SpriteIdx != spriteLifeCycle.Count - 1)
            {
                spriteRenderer.sprite = spriteLifeCycle[Data.SpriteIdx];
                Data.SpriteIdx++;
            }
            // if the current growth day is the last day thats when we can assign the last sprite
            else if (Data.CurrentGrowthDay == daysToGrow)
            {
                spriteRenderer.sprite = spriteLifeCycle[spriteLifeCycle.Count - 1];
                Data.CanHarvest = true;
            }
        }

        public void Save()
        {
            if (SaveData.Current.plantDatas == null)
            {
                SaveData.Current.plantDatas = new List<PlanteableData>();
            }
            SaveData.Current.plantDatas.Add(Data);
        }
    }
}
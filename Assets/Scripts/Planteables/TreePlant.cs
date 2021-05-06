using FarmSim.Enums;
using FarmSim.Items;
using System;
using UnityEngine;

namespace FarmSim.Planteables 
{
    /// <class name="TreePlant">
    ///     <summary>
    ///         Base class for any tree <see cref="GameObject"/>. The regrowth period of a tree
    ///         is equivalent to its sprite interval change.
    ///     </summary>
    /// </class>
    public class TreePlant : Planteable
    {
        [Header("Tree Settings")]
        [SerializeField] private ItemType logItem;
        [SerializeField] private int maxLogsToDrop = 0;
        [SerializeField] private int minLogsToDrop = 0;

        public override void Grow(int daysPassed = 1)
        {
            Debug.Log("grow tree");
            Data.CurrentGrowthDay += daysPassed;
            CheckSpriteChange();

            // if the current growth day is the last day thats when we can assign the last sprite
            if (Data.CurrentGrowthDay >= daysToGrow)
            {
                Data.CanHarvest = true;
            }
        }

        public override void OnHarvest(ToolTypes toolType, Action removePlantRef)
        {
            switch (toolType)
            {
                case ToolTypes.Sickle:
                    DropItems(itemHarvested, minAmtToDrop, maxAmtToDrop);
                    BackTrackGrowth();
                    break;
                case ToolTypes.Axe:
                    DropItems(logItem, minLogsToDrop, maxLogsToDrop);
                    RemovePlant();
                    removePlantRef?.Invoke();
                    break;
            }
        }

        private void BackTrackGrowth()
        {
            Data.CanHarvest = false;

            // back track the state of the tree by one interval
            Data.CurrentGrowthDay -= spriteChangeInterval;
            CheckSpriteChange();
        }
    }
}
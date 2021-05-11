using FarmSim.Enums;
using FarmSim.Grid;
using FarmSim.Items;
using FarmSim.Serialization;
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
        [SerializeField] private int occupyNodeIdx = 1;
        [SerializeField] private int maxLogsToDrop = 0;
        [SerializeField] private int minLogsToDrop = 0;
        [SerializeField] private int spriteIdxToMakeUnwalkable = 0;

        private NodeGrid grid;
        private INodeData nodeData;

        private void Start()
        {
            grid = FindObjectOfType<NodeGrid>();
            nodeData = grid.GetNodeFromVector2(transform.position);
            CheckNeedWater();
        }

        public override void Grow(int daysPassed = 1)
        {
            Data.CurrentGrowthDay += daysPassed;
            CheckSpriteChange();
            CheckUnwalkable();
            CheckNeedWater();

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
                    if (!CanHarvest)
                        return;

                    DropItems(itemHarvested, minAmtToDrop, maxAmtToDrop);
                    BackTrackGrowth();
                    break;
                case ToolTypes.Axe:
                    DropItems(logItem, minLogsToDrop, maxLogsToDrop);
                    RemovePlant();
                    removePlantRef?.Invoke();
                    nodeData.Data.IsWalkable = true;
                    break;
            }
        }


        /// <summary>
        ///     Once the sprite idx of the tree is >= then the given idx make the node unwalkable.
        /// </summary>
        private void CheckUnwalkable()
        {
            if(Data.SpriteIdx >= spriteIdxToMakeUnwalkable)
            {
                nodeData.Data.IsWalkable = false;
            }
        }

        /// <summary>
        ///     Once the tree has become unwalkable it no longer needs water.
        /// </summary>
        private void CheckNeedWater()
        {
            Debug.Log(nodeData.Data.IsWalkable);
            if (!nodeData.Data.IsWalkable)
            {
                NeedWater = false;
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
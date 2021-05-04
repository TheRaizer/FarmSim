using FarmSim.Enums;
using FarmSim.Items;
using UnityEngine;

namespace FarmSim.Planteables 
{
    public class TreePlant : Planteable
    {
        [Header("Tree Settings")]
        [SerializeField] private ItemType logItem;
        [SerializeField] private int maxLogsToDrop = 0;
        [SerializeField] private int minLogsToDrop = 0;

        public override void Grow(int daysPassed = 1)
        {
            base.Grow(daysPassed);
        }

        public override void OnHarvest(ToolTypes toolType)
        {
            switch (toolType)
            {
                case ToolTypes.Sickle:
                    DropItems(itemHarvested, minAmtToDrop, maxAmtToDrop);
                    // set the sprite back 1 stage
                    break;
                case ToolTypes.Axe:
                    DropItems(logItem, minLogsToDrop, maxLogsToDrop);
                    RemovePlant();
                    break;
            }
        }
    }
}
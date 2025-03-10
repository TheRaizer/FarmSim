﻿using UnityEngine;

namespace FarmSim.SavableData
{
    /// <class name="PlantableData">
    ///     <summary>
    ///         Serializable data that a Planteable contains.
    ///     </summary>
    /// </class>
    [System.Serializable]
    public class PlantableData
    {
        /// <summary>
        ///     Unique id that should be assigned to a <see cref="DirtData"/> object as well as this object as to act as a reference.
        /// </summary>
        [field: SerializeField] public string Id { get; set; }
        [field: SerializeField] public int CurrentGrowthDay { get; set; }
        [field: SerializeField] public int SpriteIdx { get; set; }
        [field: SerializeField] public bool CanHarvest { get; set; }
        [field: SerializeField] public string PrefabName { get; set; }

        public PlantableData(string _id, int _currentGrowthDay, int _spriteIdx, bool _canHarvest)
        {
            Id = _id;
            CurrentGrowthDay = _currentGrowthDay;
            SpriteIdx = _spriteIdx;
            CanHarvest = _canHarvest;
        }
    }
}
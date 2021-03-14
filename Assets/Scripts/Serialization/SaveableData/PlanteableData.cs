
namespace FarmSim.Serialization
{
    /// <class name="PlanteableData">
    ///     <summary>
    ///         Serializable data that a Planteable contains.
    ///     </summary>
    /// </class>
    [System.Serializable]
    public class PlanteableData
    {
        /// <summary>
        ///     Unique id that should be assigned to a <see cref="DirtData"/> object as well as this object as to act as a reference.
        /// </summary>
        public string Id { get; set; }
        public int CurrentGrowthDay { get; set; }
        public int SpriteIdx { get; set; }
        public bool CanHarvest { get; set; }
        public string PrefabName { get; set; }

        public PlanteableData(string _id, int _currentGrowthDay, int _spriteIdx, bool _canHarvest)
        {
            Id = _id;
            CurrentGrowthDay = _currentGrowthDay;
            SpriteIdx = _spriteIdx;
            CanHarvest = _canHarvest;
        }
    }
}
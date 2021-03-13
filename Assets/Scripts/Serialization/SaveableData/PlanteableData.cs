
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
        public string id;
        public int currentGrowthDay;
        public int spriteIdx;

        public bool canHarvest;

        public PlanteableData(string _id, int _currentGrowthDay, int _spriteIdx, bool _canHarvest)
        {
            id = _id;
            currentGrowthDay = _currentGrowthDay;
            spriteIdx = _spriteIdx;
            canHarvest = _canHarvest;
        }
    }
}
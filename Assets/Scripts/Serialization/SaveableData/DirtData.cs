
using System.Collections.Generic;

namespace FarmSim.Serialization
{
    /// <class name="DirtData">
    ///     <summary>
    ///         Serializable data that a Dirt contains.
    ///     </summary>
    /// </class>
    [System.Serializable]
    public class DirtData
    {
        /// <summary>
        ///     (If dirt references a Planteable class)
        ///     Unique id that should be assigned to a <see cref="PlanteableData"/> object as well as this object as to act as a reference.
        /// </summary>
        public string Id { get; set; }

        // The x and y help reference the corrosponding node.
        public readonly int x;
        public readonly int y;

        public bool Hoed { get; set; }
        public bool Watered { get; set; }

        /// <summary>
        ///     The number of days before the dirt reverts back to the dry from hoed.
        /// </summary>
        public int DaysTillRevert { get; set; }


        /// <summary>
        ///     List of GUIDs representing the unique WaterSources
        /// </summary>
        public List<string> WaterSrcGuids { get; } = new List<string>();

        public DirtData(string _id, int _x, int _y, bool _hoed, bool _watered, int _daysTillRevert)
        {
            Id = _id;
            x = _x;
            y = _y;
            Hoed = _hoed;
            Watered = _watered;
            DaysTillRevert = _daysTillRevert;
        }
    }
}

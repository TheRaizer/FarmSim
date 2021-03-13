
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
        public string id;

        // The x and y help reference the corrosponding node.
        public int x;
        public int y;

        public bool hoed;
        public bool watered;

        public int daysTillRevert;

        public DirtData(string _id, int _x, int _y, bool _hoed, bool _watered, int _daysTillRevert)
        {
            id = _id;
            x = _x;
            y = _y;
            hoed = _hoed;
            watered = _watered;
            daysTillRevert = _daysTillRevert;
        }
    }
}

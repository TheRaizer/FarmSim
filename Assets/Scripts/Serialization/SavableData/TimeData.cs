
namespace FarmSim.SavableData
{
    /// <class name="TimeData">
    ///     <summary>
    ///         Contains the Data relating to the time.
    ///     </summary>
    /// </class>
    [System.Serializable]
    public class TimeData
    {
        public int day = 0;

        // Singleton
        private static TimeData _current;
        public static TimeData Current
        {
            get
            {
                if (_current == null)
                {
                    _current = new TimeData();
                }
                return _current;
            }
            set
            {
                if (value != null)
                {
                    _current = value;
                }
            }
        }
    }
}

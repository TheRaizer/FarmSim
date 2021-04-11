
namespace FarmSim.Serialization
{
    /// <class name="ISaveable">
    ///     <summary>
    ///         Interface that anything needing to save data must implement.
    ///     </summary>
    /// </class>
    public interface ISavable
    {
        /// <summary>
        ///     Save some data to a data class.
        /// </summary>
        void Save();
    }
}
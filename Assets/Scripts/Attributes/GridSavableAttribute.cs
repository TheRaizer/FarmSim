
namespace FarmSim.Attributes
{
    /// <class name="SavableAttribute">
    ///     <summary>
    ///         An attribute that manages whether a savable should save on any section.
    ///     </summary>
    /// </class>
    [System.AttributeUsage(System.AttributeTargets.Class | System.AttributeTargets.Struct)]
    public class SavableAttribute : System.Attribute
    {
        // this field is assigned through attribute format
        private bool saveOnAnySection;

        public SavableAttribute(bool _saveOnAnySection)
        {
            saveOnAnySection = _saveOnAnySection;
        }
        
        // this method will be used to check during saving
        public bool GetCanSaveOnAnySection()
        {
            return saveOnAnySection;
        }
    }
}


namespace FarmSim.Attributes
{
    [System.AttributeUsage(System.AttributeTargets.Class | System.AttributeTargets.Struct)]
    public class SavableAttribute : System.Attribute
    {
        private bool saveOnAnySection;

        public SavableAttribute(bool _saveOnAnySection)
        {
            saveOnAnySection = _saveOnAnySection;
        }

        public bool GetCanSaveOnAnySection()
        {
            return saveOnAnySection;
        }
    }
}

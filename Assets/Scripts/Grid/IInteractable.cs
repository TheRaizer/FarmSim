using FarmSim.Enums;

namespace FarmSim.Grid
{
    public interface IInteractable
    {
        void OnInteract(ToolTypes toolType);
    }
}
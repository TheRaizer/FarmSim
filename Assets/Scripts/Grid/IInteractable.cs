using FarmSim.Enums;

namespace FarmSim.Grid
{
    public interface IInteractable
    {
        TileTypes TileType { get; }
        void OnInteract(ToolTypes toolType);
    }
}
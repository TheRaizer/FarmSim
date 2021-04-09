
namespace FarmSim.Slots
{
    public interface IPositionManager
    {
        SlotsHandler SlotsHandler { get; set; }
        ISwappable Swappable { get; }
    }
}

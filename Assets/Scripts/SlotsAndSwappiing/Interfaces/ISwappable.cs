using UnityEngine.UI;

namespace FarmSim.Slots
{
    public interface ISwappable
    {
        Image Icon { get; }
        int SlotIndex { get; set; }
        IPositionManager PositionManager { get; }
        bool AvoidSwap(IPositionManager other);
        void OnDestroy();
    }
}

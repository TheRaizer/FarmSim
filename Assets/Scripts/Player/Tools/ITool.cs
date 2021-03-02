
namespace FarmSim.Tools
{
    public interface ITool
    {
        int Level { get; set; }
        void OnUse();
    }
}

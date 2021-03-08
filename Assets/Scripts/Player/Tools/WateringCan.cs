using FarmSim.Enums;
using FarmSim.Grid;

namespace FarmSim.Tools
{
    public class WateringCan : ITool
    {
        private readonly NodeGrid grid;
        public WateringCan(NodeGrid _grid)
        {
            grid = _grid;
        }

        public int Level { get; set; }

        public void OnUse()
        {
            Node node = grid.GetNodeFromMousePosition();
            node.Interactable.OnInteract(ToolTypes.WATERING_CAN, null);
        }
    }
}

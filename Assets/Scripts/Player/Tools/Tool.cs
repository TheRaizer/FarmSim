using FarmSim.Enums;
using FarmSim.Grid;

namespace FarmSim.Tools
{
    public class Tool
    {
        private readonly NodeGrid grid;
        private readonly ToolTypes toolType;

        public Tool(NodeGrid _grid, ToolTypes _toolType)
        {
            grid = _grid;
            toolType = _toolType;
        }

        public int Level { get; set; }

        public void OnUse()
        {
            Node node = grid.GetNodeFromMousePosition();
            node.Interactable.OnInteract(toolType);
        }
    }
}
using FarmSim.Enums;
using FarmSim.Grid;

namespace FarmSim.Tools
{
    public class Hoe : ITool
    {
        private readonly NodeGrid grid;
        public Hoe(NodeGrid _grid)
        {
            grid = _grid;
        }

        public int Level { get; set; }

        public void OnUse()
        {
            Node node = grid.GetNodeFromMousePosition();
            node.Interactable.OnInteract(ToolTypes.HOE, null);
        }
    }
}

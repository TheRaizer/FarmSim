using FarmSim.Enums;
using FarmSim.Grid;


namespace FarmSim.Tools
{
    public class Sickle : ITool
    {
        private readonly NodeGrid grid;
        public Sickle(NodeGrid _grid)
        {
            grid = _grid;
        }

        public int Level { get; set; }

        public void OnUse()
        {
            Node node = grid.GetNodeFromMousePosition();
            node.Interactable.OnInteract(ToolTypes.Sickle);
        }
    }
}

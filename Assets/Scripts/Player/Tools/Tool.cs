using FarmSim.Enums;
using FarmSim.Grid;
using System.Collections.Generic;

namespace FarmSim.Tools
{
    public class Tool
    {
        private readonly NodeGrid grid;
        private readonly ToolTypes toolType;
        private const int DIMS_AFFECTED_INCR = 2;

        private int DimsToAffect => (Level * DIMS_AFFECTED_INCR) - 1;

        public Tool(NodeGrid _grid, ToolTypes _toolType)
        {
            grid = _grid;
            toolType = _toolType;
        }

        public int Level { get; set; } = 1;

        public void OnUse()
        {
            Node middleNode = grid.GetNodeFromMousePosition();
            List<Node> nodes = grid.GetNodesFromDimensions(middleNode, DimsToAffect, DimsToAffect);
            InteractWithNodes(nodes);
        }

        private void InteractWithNodes(List<Node> nodes)
        {
            if (nodes.Count > 0)
            {
                nodes.ForEach(node => node.Interactable.OnInteract(toolType));
            }
        }
    }
}
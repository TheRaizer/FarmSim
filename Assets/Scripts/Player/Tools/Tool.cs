using FarmSim.Enums;
using FarmSim.Grid;
using System.Collections.Generic;
using UnityEngine;

namespace FarmSim.Tools
{
    [System.Serializable]
    public class Tool
    {
        [field: SerializeField] public Sprite Sprite { get; private set; }
        [field: SerializeField] public ToolTypes ToolType { get; private set; }
        public NodeGrid Grid { get; set; }

        private const int DIMS_AFFECTED_INCR = 2;

        private int DimsToAffect => (Level * DIMS_AFFECTED_INCR) - 1;
        public int Level { get; set; } = 1;

        public void OnUse()
        {
            Node middleNode = Grid.GetNodeFromMousePosition();
            List<Node> nodes = Grid.GetNodesFromDimensions(middleNode, DimsToAffect, DimsToAffect);
            InteractWithNodes(nodes);
        }

        private void InteractWithNodes(List<Node> nodes)
        {
            if (nodes.Count > 0)
            {
                nodes.ForEach(node => node.Interactable.OnInteract(ToolType));
            }
        }
    }
}
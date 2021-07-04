using FarmSim.Enums;
using FarmSim.Grid;
using FarmSim.SavableData;
using System.Collections.Generic;
using UnityEngine;

namespace FarmSim.Tools
{
    /// <class name="Tool">
    ///     <summary>
    ///         A class that contains the attributes of a tool, as well as the general functionality.
    ///     </summary>
    /// </class>
    [System.Serializable]
    public class Tool
    {
        [field: SerializeField] public Sprite Sprite { get; private set; }
        [field: SerializeField] public ToolTypes ToolType { get; private set; }
        [field: SerializeField] public bool CanAffectNodeAhead { get; private set; }
        [SerializeField] private int numOfSounds;
        [SerializeField] private string AudioIdPrefix;

        private const int DIMS_AFFECTED_INCR = 2;

        private int DimsToAffect => (Level * DIMS_AFFECTED_INCR) - 1;
        public int Level { get; set; } = 1;

        public string GetAudioId()
        {
            int num = Random.Range(0, numOfSounds);
            return AudioIdPrefix + num;
        }

        public void OnUse(INodeData middleNode, NodeGrid nodeGrid)
        {
            List<Node> nodes = nodeGrid.GetNodesFromDimensions(middleNode, DimsToAffect, DimsToAffect);
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
using FarmSim.Enums;
using FarmSim.Grid;
using FarmSim.Loading;
using FarmSim.Tools;
using System.Collections.Generic;
using UnityEngine;

namespace FarmSim.Player
{
    public class ToolHandler : OccurPostLoad
    {
        [SerializeField] private List<Tool> toolList;
        [SerializeField] private GameObject toolPointer;

        private SpriteRenderer pointerSpriteRenderer;
        private readonly Dictionary<ToolTypes, Tool> tools = new Dictionary<ToolTypes, Tool>();

        public Node NodeToToolOn;

        public Tool EquippedTool { get; private set; }

        private PlayerController player;
        private NodeGrid grid;

        private bool detectKeys = false;

        protected override void Awake()
        {
            base.Awake();

            grid = FindObjectOfType<NodeGrid>();
            player = GetComponent<PlayerController>();
            InitTools(grid);
            pointerSpriteRenderer = toolPointer.GetComponent<SpriteRenderer>();
            EquippedTool = tools[ToolTypes.Hand];
        }

        protected override void Update()
        {
            base.Update();

            MoveToolToMouse();

            if (Input.GetMouseButtonDown(0))
            {
                NodeToToolOn = grid.GetNodeFromMousePosition();
                player.ToolToUse = EquippedTool.ToolType;
            }
        }

        private void LateUpdate()
        {
            if (detectKeys)
            {
                KeyHandler();
            }
        }

        /// <summary>
        ///     Initializes each tool from the serialized tool list into the tools dict.
        /// </summary>
        /// <param name="grid">The grid containg the possibly interactable nodes.</param>
        private void InitTools(NodeGrid grid)
        {
            toolList.ForEach(tool =>
            {
                tool.Grid = grid;
                tools.Add(tool.ToolType, tool);
            });
        }

        private void MoveToolToMouse()
        {
            Vector2 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            toolPointer.transform.position = worldPosition;
        }

        private void KeyHandler()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                EquippedTool = tools[ToolTypes.Hoe];
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                EquippedTool = tools[ToolTypes.WateringCan];
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                EquippedTool = tools[ToolTypes.Sickle];
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                EquippedTool = tools[ToolTypes.Hand];
            }
            pointerSpriteRenderer.sprite = EquippedTool.Sprite;
        }

        // animation event to use during tool animations
        private void UseTool()
        {
            tools[player.ToolToUse].OnUse(NodeToToolOn);
        }

        protected override void PostLoad()
        {
            detectKeys = true;
        }
    }
}
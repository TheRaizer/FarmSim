using FarmSim.Enums;
using FarmSim.Grid;
using FarmSim.Loading;
using FarmSim.Tools;
using FarmSim.Utility;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FarmSim.Player
{
    public class ToolHandler : MonoBehaviour, IOccurPostLoad
    {
        [SerializeField] private List<Tool> toolList;
        [SerializeField] private Image toolPointer;
        private readonly Dictionary<ToolTypes, Tool> tools = new Dictionary<ToolTypes, Tool>();

        public Node NodeToToolOn;
        public Tool EquippedTool { get; private set; }

        private PlayerController player;
        private Canvas canvas;

        private bool detectKeys = false;

        private void Awake()
        {
            player = GetComponent<PlayerController>();
            canvas = FindObjectOfType<Canvas>();

            InitTools();
            EquippedTool = tools[ToolTypes.Hand];
        }

        private void Update()
        {
            MoveToolToMouse();

            if (Input.GetMouseButtonDown(0))
            {
                Node node = NodeGrid.Instance.GetNodeFromMousePosition();
                if(node != null)
                {
                    NodeToToolOn = node;
                }
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
        private void InitTools()
        {
            toolList.ForEach(tool =>
            {
                tools.Add(tool.ToolType, tool);
            });
        }

        private void MoveToolToMouse()
        {
            toolPointer.rectTransform.SetToMouse(canvas);
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
            toolPointer.sprite = EquippedTool.Sprite;
        }

        // animation event to use during tool animations
        private void UseTool()
        {
            Tool tool = tools[player.ToolToUse];
            AudioManager.Instance.Play(tool.GetAudioId());
            tool.OnUse(NodeToToolOn);
        }

        public void PostLoad()
        {
            detectKeys = true;
        }
    }
}
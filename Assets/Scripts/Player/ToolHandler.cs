﻿using FarmSim.Enums;
using FarmSim.Grid;
using FarmSim.SavableData;
using FarmSim.Serialization;
using FarmSim.Tools;
using FarmSim.Utility;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FarmSim.Player
{
    /// <class name="ToolHandler">
    ///     <summary>
    ///         Handles the use and currently equipped tool.
    ///     </summary>
    /// </class>
    public class ToolHandler : MonoBehaviour, IOccurPostLoad
    {
        [SerializeField] private List<Tool> toolList;
        [SerializeField] private Image toolPointer;
        private readonly Dictionary<ToolTypes, Tool> tools = new Dictionary<ToolTypes, Tool>();

        private INodeData nodeToToolOn;

        /// <summary>
        ///     The tool that is currently equipped by the player.
        /// </summary>
        public Tool EquippedTool { get; private set; }

        /// <summary>
        ///     In contrast to the <see cref="ToolHandler.EquippedTool"/>, this is the 
        ///     one that is currently in line to be used (not neccesesarily the one that is equipped).
        /// </summary>
        public ToolTypes ToolToUse { get; private set; }

        private Canvas canvas;
        private NodeGrid nodeGrid;
        private AudioManager audioManager;

        private bool detectKeys = false;

        private void Awake()
        {
            nodeGrid = FindObjectOfType<NodeGrid>();
            canvas = FindObjectOfType<Canvas>();
            audioManager = FindObjectOfType<AudioManager>();

            InitTools();
            EquippedTool = tools[ToolTypes.Hand];
        }

        private void Update()
        {
            MoveToolToMouse();

            if (Input.GetMouseButtonDown(0))
            {
                INodeData node = nodeGrid.GetNodeFromMousePosition();
                if (node != null)
                {
                    nodeToToolOn = node;
                }
                ToolToUse = EquippedTool.ToolType;
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
            else if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                EquippedTool = tools[ToolTypes.Axe];
            }
            toolPointer.sprite = EquippedTool.Sprite;
        }

        // animation event to use during tool animations
        private void UseTool()
        {
            Tool tool = tools[ToolToUse];
            audioManager.Play(tool.GetAudioId());
            tool.OnUse(nodeToToolOn, nodeGrid);
        }

        public void PostLoad()
        {
            detectKeys = true;
        }

        public Tool GetToolToUse()
        {
            return tools[ToolToUse];
        }
    }
}
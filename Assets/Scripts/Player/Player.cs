using FarmSim.Enums;
using FarmSim.Grid;
using FarmSim.Tools;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace FarmSim.Player
{
    public class Player : MonoBehaviour
    {
        private readonly Dictionary<ToolTypes, Tool> tools = new Dictionary<ToolTypes, Tool>();
        private Tool equippedTool;

        private void Awake()
        {
            NodeGrid grid = GetComponent<NodeGrid>();

            foreach(ToolTypes tool in Enum.GetValues(typeof(ToolTypes)))
            {
                tools.Add(tool, new Tool(grid, tool));
            }
        }

        private void LateUpdate()
        {
            OnMouseClick();
        }

        private void OnMouseClick()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (equippedTool != null)
                {
                    equippedTool.OnUse();
                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                equippedTool = tools[ToolTypes.Hoe];
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                equippedTool = tools[ToolTypes.WateringCan];
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                equippedTool = tools[ToolTypes.Sickle];
            }
        }
    }
}

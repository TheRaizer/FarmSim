using FarmSim.Grid;
using FarmSim.Tools;
using System.Collections.Generic;
using UnityEngine;

namespace FarmSim.Player
{
    public class Player : MonoBehaviour
    {
        private ITool tool;
        private Hoe hoe;
        private WateringCan wateringCan;
        private Sickle sickle;

        private void Awake()
        {
            NodeGrid grid = GetComponent<NodeGrid>();
            hoe = new Hoe(grid);
            wateringCan = new WateringCan(grid);
            sickle = new Sickle(grid);
        }

        private void LateUpdate()
        {
            OnMouseClick();
        }

        private void OnMouseClick()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (tool != null)
                {
                    tool.OnUse();
                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                tool = hoe;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                tool = wateringCan;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                tool = sickle;
            }
        }
    }
}

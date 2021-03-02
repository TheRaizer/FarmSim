using FarmSim.Grid;
using FarmSim.Tools;
using UnityEngine;

namespace FarmSim.Player
{
    public class Player : MonoBehaviour
    {
        private ITool tool;

        private void Awake()
        {
            //TEST LINE
            tool = new Hoe(GetComponent<NodeGrid>());
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
        }
    }
}

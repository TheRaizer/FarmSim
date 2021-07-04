using FarmSim.Attributes;
using FarmSim.Entity;
using FarmSim.Enums;
using FarmSim.Grid;
using FarmSim.SavableData;
using FarmSim.Serialization;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace FarmSim.Player
{
    /// <class name="PlayerController">
    ///     <summary>
    ///         Controls and manages the components of the Player.
    ///     </summary>
    /// </class>
    [Savable(true)]
    public class PlayerController : MonoBehaviour, ISavable, ILoadable
    {
        [SerializeField] private float speed;
        [SerializeField] private GameObject tileRing;

        private const string WALKING_ANIM = "Walking";

        public Action OnPlant { private get; set; }

        private ToolHandler toolHandler;
        private Animator animator;
        private EntityPathFind pathFind;
        private NodeGrid nodeGrid;

        private void Awake()
        {
            toolHandler = GetComponent<ToolHandler>();
            nodeGrid = FindObjectOfType<NodeGrid>();
            // on pathfind fail and success trigger the animation due to tools that interact one node ahead
            pathFind = new EntityPathFind(TriggerAnimation, TriggerAnimation, gameObject, nodeGrid, FindObjectOfType<PathRequestManager>(), speed);
            //to hide the curser
            Cursor.visible = false;
            animator = GetComponent<Animator>();
        }

        private void LateUpdate()
        {
            KeyHandler();
            ChangeRingPosition();
        }

        private void KeyHandler()
        {
            if (pathFind.HasPath())
            {
                if (Input.GetKeyDown(KeyCode.S))
                {
                    pathFind.StopMoving();
                }
            }

            // check if mouse is pressed and if the pointer is not on a UI object
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                pathFind.RequestPath();
            }
            pathFind.MoveOnPath();
        }

        private void ChangeRingPosition()
        {
            INodeData node = nodeGrid.GetNodeFromMousePosition();
            if (node != null)
            {
                Vector2 pos = node.Data.pos;
                tileRing.transform.position = pos;
            }
        }

        private bool CheckForPlanting()
        {
            // if there is an action for on plant that means we must plant something
            if (OnPlant != null)
            {
                // invoke the action
                OnPlant.Invoke();

                // empty the action
                OnPlant = null;

                return true;
            }

            return false;
        }

        public void TriggerAnimation(INodeData curr, INodeData end, bool succesful)
        {
            if (succesful)
            {
                // if we have planted return
                if (CheckForPlanting())
                    return;
            }
            else
            {

                // if the pathfinding failed because the end node wasnt walkable
                if (end != null && !end.Data.IsWalkable)
                {
                    bool isNeighbour = nodeGrid.GetCardinalNeighbours(curr).Contains((Node)end);

                    // if the node is in the vicinity of the player and the player is not moving on a path
                    if (!pathFind.HasPath() && isNeighbour)
                    {
                        animator.SetBool(WALKING_ANIM, false);

                        // if target node didnt change thats fine because the player will continue to look in the same direction.
                        pathFind.ChangeDir(end.Data.pos);

                        // if the tool cannot affect the node ahead dont use tool
                        if (!toolHandler.GetToolToUse().CanAffectNodeAhead)
                            return;
                    }
                    else
                        return;
                }
                else
                    return;
            }
            ChooseAnimation(toolHandler.ToolToUse);
        }

        private void ChooseAnimation(ToolTypes toolType)
        {
            switch (toolType)
            {
                case ToolTypes.Hoe:
                    animator.SetTrigger("Hoe");
                    break;
                case ToolTypes.WateringCan:
                    animator.SetTrigger("Water");
                    break;
                case ToolTypes.Sickle:
                    animator.SetTrigger("Sickle");
                    break;
                case ToolTypes.Axe:
                    animator.SetTrigger("Axe");
                    break;
                default:
                    break;
            }
        }

        public void Save()
        {
            PlayerData.Current.position = transform.position;
        }

        public void Load()
        {
            if (PlayerData.Current.position == Vector2.zero)
                return;

            transform.position = PlayerData.Current.position;
        }

        public CardinalDirections GetPlayerDir()
        {
            return pathFind.Dir;
        }
    }
}

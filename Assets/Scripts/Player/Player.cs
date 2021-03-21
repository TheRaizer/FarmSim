using FarmSim.Enums;
using FarmSim.Grid;
using System;
using UnityEngine;

namespace FarmSim.Player
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private float speed;

        private Animator animator;
        private ToolHandler toolHandler;

        private CardinalDirections dir = CardinalDirections.South;

        private NodeGrid grid;
        private Vector2[] path;
        private PathRequest currentRequest;

        private bool processingPath = false;

        private int pathIdx = 0;

        private void Awake()
        {
            //to hide the curser
            Cursor.visible = false;

            toolHandler = GetComponent<ToolHandler>();
            grid = FindObjectOfType<NodeGrid>();
            animator = GetComponent<Animator>();
        }

        private void Update()
        {
            if(processingPath && Input.GetKeyDown(KeyCode.S))
            {
                PathRequestManager.Instance.StopSearch(currentRequest.id);
                path = null;
            }
            MoveOnPath();

            if (Input.GetMouseButtonDown(0))
            {
                toolHandler.NodeToTool = grid.GetNodeFromMousePosition();
                RequestPath();
            }
        }

        private void ChangeDir(Vector2 next)
        {
            Vector2 travelDir = next - (Vector2)transform.position;

            if (travelDir.x > 0)
            {
                dir = CardinalDirections.East;
            }
            else if (travelDir.x < 0)
            {
                dir = CardinalDirections.West;
            }
            else if(travelDir.y > 0)
            {
                dir = CardinalDirections.North;
            }
            else if(travelDir.y < 0)
            {
                dir = CardinalDirections.South;
            }

            animator.SetInteger("Direction", (int)dir);
        }

        private void MoveOnPath()
        {
            if (path != null)
            {
                animator.SetBool("Walking", true);
                if (pathIdx < path.Length)
                {
                    Vector2 curr = gameObject.transform.position;
                    Vector2 target = path[pathIdx];

                    ChangeDir(target);

                    if (Mathf.Abs(curr.x - target.x) < 0.01f && Mathf.Abs(curr.y - target.y) < 0.01f)
                    {
                        pathIdx++;
                    }
                    gameObject.transform.position = Vector2.MoveTowards(curr, target, speed * Time.deltaTime);
                }
                else
                {
                    // it has completed the path
                    animator.SetBool("Walking", false);
                    TriggerToolAnimation();
                    path = null;
                }
            }
        }

        public void TriggerToolAnimation()
        {
            switch (toolHandler.EquippedTool.ToolType)
            {
                case ToolTypes.Hoe:
                    animator.SetTrigger("Hoe");
                    Debug.Log("Set t");
                    break;
                case ToolTypes.WateringCan:
                    // set trigger to water. In that animation run the ToolHandler.UsTool() as event.
                    break;
                case ToolTypes.Sickle:
                    // set trigger to sickle. In that animation run the ToolHandler.UsTool() as event.
                default:
                    break;
            }
        }

        private void PathFindCallBack(Vector2[] path, bool isSuccesful)
        {
            pathIdx = 0;
            if (isSuccesful)
            {
                this.path = path;
            }
            else
            {
                this.path = null;
            }

            processingPath = false;
        }

        private void RequestPath()
        {
            Debug.Log("request path");
            Node start = grid.GetNodeFromVector2(gameObject.transform.position);
            Node end = grid.GetNodeFromMousePosition();

            currentRequest = new PathRequest(Guid.NewGuid().ToString(), start, end, PathFindCallBack);

            PathRequestManager.Instance.RequestPath(currentRequest);
            processingPath = true;
        }
    }
}

﻿using FarmSim.Enums;
using FarmSim.Grid;
using System;
using UnityEngine;

namespace FarmSim.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private GameObject tileRing; 

        public Action OnPlant { private get; set; }
        public ToolTypes ToolToUse { get; set; }
        public Node Destination { get; private set; }

        private Animator animator;
        private CardinalDirections dir = CardinalDirections.South;

        private NodeGrid grid;
        private Vector2[] path;
        private PathRequest currentRequest;

        private bool processingPath = false;
        private bool stop = false;

        private int pathIdx = 0;

        private void Awake()
        {
            //to hide the curser
            Cursor.visible = false;

            grid = FindObjectOfType<NodeGrid>();
            animator = GetComponent<Animator>();
        }

        private void Update()
        {
            if(path != null)
            {
                if (Input.GetKeyDown(KeyCode.S))
                {
                    stop = true;
                    if (processingPath)
                        PathRequestManager.Instance.StopSearch(currentRequest.id);
                }
            }
            else
            {
                Vector2 pos = grid.GetNodeFromMousePosition().Data.pos;
                tileRing.transform.position = pos;
            }

            
            MoveOnPath();

            if (Input.GetMouseButtonDown(0))
            {
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
                        if (stop)
                        {
                            // stop early
                            animator.SetBool("Walking", false);
                            path = null;
                            stop = false;
                        }
                        pathIdx++;
                    }
                    gameObject.transform.position = Vector2.MoveTowards(curr, target, speed * Time.deltaTime);
                }
                else
                {
                    // it has completed the path
                    animator.SetBool("Walking", false);
                    TriggerAnimation();
                    path = null;
                }
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

        public void TriggerAnimation()
        {
            // if we have planted return
            if (CheckForPlanting())
                return;


            // otherwise check if a tool was used
            switch (ToolToUse)
            {
                case ToolTypes.Hoe:
                    animator.SetTrigger("Hoe");
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
            Node start = grid.GetNodeFromVector2(gameObject.transform.position);
            Node end = grid.GetNodeFromMousePosition();

            Destination = end;

            currentRequest = new PathRequest(Guid.NewGuid().ToString(), start, end, PathFindCallBack);

            PathRequestManager.Instance.RequestPath(currentRequest);
            processingPath = true;
        }
    }
}

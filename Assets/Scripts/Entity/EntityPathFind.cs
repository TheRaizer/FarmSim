using FarmSim.Enums;
using FarmSim.Grid;
using System;
using UnityEngine;

namespace FarmSim.Entity
{
    /// <class name="EntityPathFind">
    ///     <summary>
    ///         Manages pathfinding requests and movement of an entity.
    ///     </summary>
    /// </class>
    public class EntityPathFind
    {
        public Node Destination { get; private set; }
        public bool ProcessingPath { get; private set; }
        public float Speed { private get; set; }

        private CardinalDirections dir = CardinalDirections.South;
        private Vector2[] path;
        private PathRequest currentRequest;

        private bool stop = false;

        private int pathIdx = 0;

        private readonly Action onArrival;
        private readonly Transform transform;
        private readonly GameObject gameObject;
        private readonly Animator animator;

        private readonly string walkingBoolAnimTag;
        private readonly string directionIntTag;


        public EntityPathFind(Action _onArrival, GameObject _gameObject, float speed = 10, string _walkingBoolAnimTag = "Walking", string _directionIntTag = "Direction")
        {
            onArrival = _onArrival;
            gameObject = _gameObject;
            Speed = speed;
            walkingBoolAnimTag = _walkingBoolAnimTag;
            directionIntTag = _directionIntTag;

            transform = gameObject.transform;
            animator = gameObject.GetComponent<Animator>();
        }

        public void RequestPath()
        {
            Node start;

            // if there is a path and the index were moving too is still valid use it as the start
            if (path != null && path.Length > pathIdx)
            {
                // this smooths some transitions
                start = NodeGrid.Instance.GetNodeFromVector2(path[pathIdx]);
            }
            else
            {
                start = NodeGrid.Instance.GetNodeFromVector2(transform.position);
            }
            
            Node end = NodeGrid.Instance.GetNodeFromMousePosition();

            if (start == null || end == null || !end.Data.IsWalkable)
                return;

            Destination = end;
            currentRequest = new PathRequest(Guid.NewGuid().ToString(), start, end, PathFindCallBack);

            PathRequestManager.Instance.RequestPath(currentRequest);
            ProcessingPath = true;
        }

        public void MoveOnPath()
        {
            if (path != null)
            {
                animator.SetBool(walkingBoolAnimTag, true);
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
                            animator.SetBool(walkingBoolAnimTag, false);
                            path = null;
                            stop = false;
                        }
                        pathIdx++;
                    }
                    gameObject.transform.position = Vector2.MoveTowards(curr, target, Speed * Time.deltaTime);
                }
                else
                {
                    // it has completed the path
                    animator.SetBool(walkingBoolAnimTag, false);
                    onArrival?.Invoke();
                    path = null;
                }
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
            else if (travelDir.y > 0)
            {
                dir = CardinalDirections.North;
            }
            else if (travelDir.y < 0)
            {
                dir = CardinalDirections.South;
            }

            animator.SetInteger(directionIntTag, (int)dir);
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

            ProcessingPath = false;
        }

        public bool HasPath() => path != null;
        public void StopMoving()
        {
            stop = true;
            if (ProcessingPath)
                PathRequestManager.Instance.StopSearch(currentRequest.id);
        }
    }
}
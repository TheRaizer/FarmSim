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
        public bool ProcessingPath { get; private set; }
        public float Speed { private get; set; }

        public CardinalDirections Dir { get; private set; } = CardinalDirections.South;
        private Vector2[] path;
        private PathRequest currentRequest;

        private bool stop = false;

        private int pathIdx = 0;

        private readonly Action onArrival;
        private readonly Action onFail;

        private readonly Transform transform;
        private readonly GameObject gameObject;
        private readonly Animator animator;
        private readonly NodeGrid nodeGrid;
        private readonly PathRequestManager requestManager;

        private readonly string walkingBoolAnimTag;
        private readonly string directionIntTag;

        private Vector2 targetNodePos = Vector2.zero;

        public EntityPathFind(Action _onFail, Action _onArrival, GameObject _gameObject, NodeGrid _nodeGrid, PathRequestManager _requestManager, float speed = 10, string _walkingBoolAnimTag = "Walking", string _directionIntTag = "Direction")
        {
            onArrival = _onArrival;
            onFail = _onFail;

            gameObject = _gameObject;
            Speed = speed;
            walkingBoolAnimTag = _walkingBoolAnimTag;
            directionIntTag = _directionIntTag;
            nodeGrid = _nodeGrid;
            requestManager = _requestManager;

            transform = gameObject.transform;
            animator = gameObject.GetComponent<Animator>();
        }

        public void RequestPath()
        {
            Node start = nodeGrid.GetNodeFromVector2(transform.position);
            Node end = nodeGrid.GetNodeFromMousePosition();

            if(end != null)
            {
                // store the target node's position so we can change the players direction to face it.
                targetNodePos = end.Data.pos;
            }

            if (start == null || end == null)
                return;

            currentRequest = new PathRequest(Guid.NewGuid().ToString(), start, end, PathFindCallBack);

            requestManager.RequestPath(currentRequest);
            ProcessingPath = true;
        }

        public void MoveOnPath()
        {
            if (path != null)
            {
                // set the walking animation
                animator.SetBool(walkingBoolAnimTag, true);

                // if we have not completed the path
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
                Dir = CardinalDirections.East;
            }
            else if (travelDir.x < 0)
            {
                Dir = CardinalDirections.West;
            }
            else if (travelDir.y > 0)
            {
                Dir = CardinalDirections.North;
            }
            else if (travelDir.y < 0)
            {
                Dir = CardinalDirections.South;
            }

            animator.SetInteger(directionIntTag, (int)Dir);
        }

        private void PathFindCallBack(Vector2[] path, bool foundPath)
        {
            pathIdx = 0;
            if (foundPath)
            {
                this.path = path;
            }
            else
            {
                animator.SetBool(walkingBoolAnimTag, false);
                // if target node didnt change thats fine because the player will continue to look in the same direction.
                ChangeDir(targetNodePos);

                // call on arrival for tools that interact one node ahead
                onFail?.Invoke();
                this.path = null;
            }

            ProcessingPath = false;
        }

        public bool HasPath() => path != null;
        public void StopMoving()
        {
            stop = true;
            if (ProcessingPath)
                requestManager.StopSearch(currentRequest.id);
        }
    }
}
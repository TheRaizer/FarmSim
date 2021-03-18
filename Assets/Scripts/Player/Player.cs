using FarmSim.Grid;
using System.Collections.Generic;
using UnityEngine;

namespace FarmSim.Player
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private float speed;
        private NodeGrid grid;
        private Vector2[] path;
        private PathRequest currentRequest;

        private bool processingPath = false;

        private int pathIdx = 0;

        private void Awake()
        {
            //to hide the curser
            Cursor.visible = false;
            grid = FindObjectOfType<NodeGrid>();
        }

        private void Update()
        {
            if(processingPath && Input.GetKeyDown(KeyCode.S))
            {
                PathRequestManager.Instance.StopSearch(currentRequest.id);
            }
            MoveOnPath();

            if (Input.GetMouseButtonDown(0))
            {
                RequestPath();
            }
        }

        private void MoveOnPath()
        {
            if (path != null)
            {
                if (pathIdx < path.Length)
                {
                    Vector2 curr = gameObject.transform.position;
                    Vector2 target = path[pathIdx];

                    if (Mathf.Abs(curr.x - target.x) < 0.01f && Mathf.Abs(curr.y - target.y) < 0.01f)
                    {
                        pathIdx++;
                    }
                    gameObject.transform.position = Vector2.MoveTowards(curr, target, speed * Time.deltaTime);
                }
            }
        }

        private void PathFindCallBack(Vector2[] path, bool isSuccesful)
        {
            Debug.Log("Path ran " + isSuccesful);
            if (path != null)
                Debug.Log("Path length: " + path.Length);
            else
                Debug.Log("no path");
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

            currentRequest = new PathRequest(System.Guid.NewGuid().ToString(), start, end, PathFindCallBack);

            PathRequestManager.Instance.RequestPath(currentRequest);
            processingPath = true;
        }
    }
}

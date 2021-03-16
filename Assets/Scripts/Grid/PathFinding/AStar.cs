using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FarmSim.Grid
{
    class AStar
    {
        private readonly NodeGrid grid;

        private bool stopSearch = false;
        private string idToStop = "";

        public AStar(NodeGrid _grid)
        {
            grid = _grid;
        }

        public void StopSearch(string id)
        {
            stopSearch = true;
            idToStop = id;
        }

        private Vector2[] RetracePath(Node start, Node end)
        {
            // holds vector2 list from end node to start node
            List<Node> backwardPath = new List<Node>();
            Node curr = end;

            while (curr != start)
            {
                backwardPath.Add(curr);
                curr = curr.parentNode;
            }

            // flip the backward path to be the proper path
            backwardPath.Reverse();

            // simplify the path
            Vector2[] path = SimplifyPath(backwardPath);

            return path;
        }

        /// <summary>
        ///     Simplifies the path to only contain Vector2's where a direction change is to be made.
        ///     <remarks>
        ///         <para>
        ///             The way we check if the direction is different utilizes the fact that 
        ///             each node is always only up to 1X and 1Y distance away from each other.
        ///         </para>
        ///         <para>
        ///             This would mean that a Node cannot be in the same direction with a greater magnitude.
        ///         </para>
        ///     </remarks>
        /// </summary>
        /// <param name="path">Path from the start to end nodes.</param>
        /// <returns>A simplified path containing only nodes where direction has changed.</returns>
        Vector2[] SimplifyPath(List<Node> path)
        {
            List<Vector2> simplePath = new List<Vector2>();
            Vector2 directionOld = Vector2.zero;

            for (int i = 1; i < path.Count; i++)
            {
                Node curr = path[i];
                Node prev = path[i - 1];

                // get the new direction from the prev node to the current
                Vector2 directionNew = new Vector2(prev.Data.x - curr.Data.x, prev.Data.y - curr.Data.y);

                // check if the direction has changed
                if (directionNew != directionOld)
                {
                    // add to the path if direction has changed
                    simplePath.Add(path[i].Data.pos);
                }
                directionOld = directionNew;
            }
            return simplePath.ToArray();
        }

        public IEnumerator PathFind(Node start, Node end, string id)
        {
            if (!start.Data.IsWalkable && !end.Data.IsWalkable)
            {
                yield break;
            }
            Heap<Node> openHeap = new Heap<Node>(NodeGrid.SECTION_SIZE_X * NodeGrid.SECTION_SIZE_Y);
            HashSet<Node> closedSet = new HashSet<Node>();

            start.parentNode = start;

            openHeap.Add(start);

            bool foundPath = false;

            while(openHeap.Count > 0)
            {
                Node currentNode = openHeap.RemoveFirst();

                if (currentNode == end)
                {
                    foundPath = true;
                    break;
                }
                else if (stopSearch && id == idToStop)
                {
                    stopSearch = false;
                    break;
                }

                List<Node> neighbours = grid.GetMooreNeighbours(currentNode);

                foreach (Node neighbour in neighbours)
                {
                    if (closedSet.Contains(neighbour) || !neighbour.Data.IsWalkable)
                    {
                        continue;
                    }

                    int distCurrToNeighbour = grid.GetDistance(currentNode, neighbour);

                    // the new gcost of the neighbour to the start through the current node
                    int newNeighbourGCost = currentNode.gCost + distCurrToNeighbour;

                    if(!openHeap.Contains(neighbour) || newNeighbourGCost < neighbour.gCost)
                    {
                        neighbour.gCost = newNeighbourGCost;
                        neighbour.parentNode = currentNode;

                        if (!openHeap.Contains(neighbour))
                        {
                            neighbour.hCost = grid.GetDistance(neighbour, end);
                            openHeap.Add(neighbour);
                        }
                        else
                        {
                            openHeap.UpdateItem(neighbour);
                        }
                    }
                }

                closedSet.Add(currentNode);
                yield return null;
            }

            Vector2[] path = null;
            if (foundPath)
            {
                Debug.Log("path found");
                path = RetracePath(start, end);
            }
            else
            {
                Debug.Log("NO path found");
            }

            PathRequestManager.Instance.OnFinishProcess(path, foundPath);
        }
    }
}

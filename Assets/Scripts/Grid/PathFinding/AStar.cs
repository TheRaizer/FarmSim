using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FarmSim.Grid
{
    /// <class name="AStar">
    ///     <summary>
    ///         Contains functions for pathfinding using the A* algorithm.
    ///     </summary>
    /// </class>
    public class AStar
    {
        private readonly NodeGrid grid;
        private readonly PathRequestManager requestManager;

        private bool stopSearch = false;
        private string idToStop = "";

        public AStar(NodeGrid _grid, PathRequestManager _requestManager)
        {
            grid = _grid;
            requestManager = _requestManager;
        }

        public void StopSearch(string id)
        {
            stopSearch = true;
            idToStop = id;
        }

        private Vector2[] RetracePath(Node start, Node end)
        {
            // holds vector2 list from end node to start node
            List<Vector2> backwardPath = new List<Vector2>();
            Node curr = end;

            while (curr != start)
            {
                backwardPath.Add(curr.Data.pos);
                curr = curr.parentNode;
            }

            // flip the backward path to be the proper path
            backwardPath.Reverse();

            // simplify the path
            Vector2[] path = backwardPath.ToArray();

            return path;
        }

        public IEnumerator PathFindCo(Node start, Node end, string id)
        {
            if (!start.Data.IsWalkable && !end.Data.IsWalkable)
            {
                requestManager.OnFinishProcess(null, false);
                yield break;
            }
            Heap<Node> openHeap = new Heap<Node>(NodeGrid.SECTION_SIZE_X * NodeGrid.SECTION_SIZE_Y);
            HashSet<Node> closedSet = new HashSet<Node>();

            start.parentNode = start;

            openHeap.Add(start);

            bool foundPath = false;

            while (openHeap.Count > 0)
            {
                Node currentNode = openHeap.RemoveFirst();
                closedSet.Add(currentNode);

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

                List<Node> neighbours = grid.GetCardinalNeighbours(currentNode);

                foreach (Node neighbour in neighbours)
                {
                    if (closedSet.Contains(neighbour) || !neighbour.Data.IsWalkable)
                    {
                        continue;
                    }

                    int distCurrToNeighbour = grid.GetManhattanDistance(currentNode, neighbour);

                    // the new gcost of the neighbour to the start through the current node
                    int newNeighbourGCost = currentNode.gCost + distCurrToNeighbour;

                    if (!openHeap.Contains(neighbour) || newNeighbourGCost < neighbour.gCost)
                    {
                        neighbour.gCost = newNeighbourGCost;
                        neighbour.parentNode = currentNode;

                        if (!openHeap.Contains(neighbour))
                        {
                            neighbour.hCost = grid.GetManhattanDistance(neighbour, end);
                            openHeap.Add(neighbour);
                        }
                        else
                        {
                            openHeap.UpdateItem(neighbour);
                        }
                    }
                }
            }


            yield return null;
            Vector2[] path = null;
            if (foundPath)
            {
                path = RetracePath(start, end);
            }

            requestManager.OnFinishProcess(path, foundPath);
        }
    }
}

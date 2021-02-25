using System;
using UnityEngine;

namespace FarmSim.Grid
{
    /// <class name="GridLayout">
    ///     <summary>
    ///         Contains a grid of <see cref="Node"/>'s and functions
    ///         that manage them.
    ///     </summary>
    /// </class>

    public class NodeGrid : MonoBehaviour
    {
        [SerializeField] private float gridWorldX = 50;
        [SerializeField] private float gridWorldY = 50;

        private int gridMaxX = 0;
        private int gridMaxY = 0;

        private Node[,] grid;

        private void Awake()
        {
            gridMaxX = Mathf.FloorToInt(gridWorldX / Node.NODE_DIAMETER);
            gridMaxY = Mathf.FloorToInt(gridWorldY / Node.NODE_DIAMETER);

            grid = new Node[gridMaxX, gridMaxY];
            InitGrid();
        }


        /// <summary>
        ///     Obtains a <see cref="Node"/> from the <see cref="grid"/> given a <see cref="Vector2"/>.
        /// </summary>
        /// <param name="vector">The vector to find a Node from</param>
        /// <returns>
        ///     <see cref="Node"/> that is located in the given <see cref="Vector2"/>.
        ///     Null if vector is not part of a node.
        /// </returns>
        public Node GetNodeFromVector2(Vector2 vector)
        {
            int x = Mathf.FloorToInt((vector.x - transform.position.x) / Node.NODE_DIAMETER);
            int y = Mathf.FloorToInt((vector.y - transform.position.y) / Node.NODE_DIAMETER);

            if (IsInGrid(x, y))
            {
                return grid[x, y];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        ///     Checks every node in a certain space around a given node and returns if something can be placed.
        /// </summary>
        /// <param name="node">The node whose at the center of the dimensions.</param>
        /// <param name="xDim">The x-dimension to check.</param>
        /// <param name="yDim">The y-dimension to check.</param>
        /// <returns>true if there are no occupied Nodes in the space, otherwise false.</returns>
        public bool IsValidPlacement(Node node, int xDim, int yDim)
        {
            int yStart = node.y - yDim / 2;
            int xStart = node.x - xDim / 2;

            for (int y = 0; y < yDim; y++)
            {
                for (int x = 0; x < xDim; x++)
                {
                    int nodeX = x + xStart;
                    int nodeY = y + yStart;
                    if (IsInGrid(nodeX, nodeY) && grid[nodeX, nodeY].IsOccupied)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        ///     Occupies every node within certain dimensions around a given node.
        /// </summary>
        /// <param name="node">The node the dimensions surround.</param>
        /// <param name="xDim">The x-dimension.</param>
        /// <param name="yDim">The y-dimension.</param>
        public void MakeDimensionsOccupied(Node node, int xDim, int yDim)
        {
            int yStart = node.y - yDim / 2;
            int xStart = node.x - xDim / 2;

            for (int y = 0; y < yDim; y++)
            {
                for (int x = 0; x < xDim; x++)
                {
                    int nodeX = x + xStart;
                    int nodeY = y + yStart;
                    if (IsInGrid(nodeX, nodeY))
                    {
                        grid[nodeX, nodeY].IsOccupied = true;
                    }

                }
            }
        }

        /// <summary>
        ///     Finds if a given x and y indices are in the grid.
        /// </summary>
        /// <param name="x">x-index</param>
        /// <param name="y">y-index</param>
        /// <returns>true if it is in the grid, otherwise false</returns>
        private bool IsInGrid(int x, int y)
        {
            if (x < gridMaxX && y < gridMaxY && x >= 0 && y >= 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Initialize's nodes into the <see cref="grid"/> and assigns a 
        /// world position to each.
        /// </summary>
        private void InitGrid()
        {
            for (int y = 0; y < gridMaxY; y++)
            {
                for (int x = 0; x < gridMaxX; x++)
                {
                    Vector2 pos = GetNodePosition(x, y);
                    grid[x, y] = new Node(pos, x, y);
                }
            }
        }

        /// <summary>
        ///     Gets the nodes position given x and y indices.
        /// </summary>
        /// <param name="x">x-index</param>
        /// <param name="y">y-index</param>
        /// <returns><see cref="Vector2"/> position of a node at indices x and y</returns>
        private Vector2 GetNodePosition(int x, int y)
        {
            float xPos = x * Node.NODE_DIAMETER + Node.NODE_RADIUS;
            float yPos = y * Node.NODE_DIAMETER + Node.NODE_RADIUS;

            Vector2 pos = new Vector2(xPos + transform.position.x, yPos + transform.position.y);
            return pos;
        }

        private void OnDrawGizmos()
        {
            if (Application.isPlaying)
            {
                for (int y = 0; y < gridMaxY; y++)
                {
                    for (int x = 0; x < gridMaxX; x++)
                    {
                        Gizmos.DrawSphere(grid[x, y].Position, 0.1f);
                        Gizmos.DrawWireCube(grid[x, y].Position, new Vector3(Node.NODE_DIAMETER, Node.NODE_DIAMETER, 0));
                    }
                }
            }
        }
    }
}

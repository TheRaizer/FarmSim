using FarmSim.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
        [SerializeField] private int sectionNum = 0;

        private readonly int worldMaxX = 150;
        private readonly int worldMaxY = 150;

        private const int SECTION_SIZE_X = 30;
        private const int SECTION_SIZE_Y = 30;

        private Node[,] grid;
        private WorldLoader worldLoader = null;
        public bool LoadedSection { get; private set; } = false;

        private void Awake()
        {
            worldLoader = new WorldLoader(transform.position, sectionNum, FindObjectOfType<ObjectPooler>());
            grid = worldLoader.InitGrid();
        }

        private void Start()
        {
            StartCoroutine(worldLoader.LoadSection(grid, () => LoadedSection = true));
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

            if (IsInSection(x, y))
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
                    if (IsInSection(nodeX, nodeY) && grid[nodeX, nodeY].IsOccupied)
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
                    if (IsInSection(nodeX, nodeY))
                    {
                        grid[nodeX, nodeY].IsOccupied = true;
                    }

                }
            }
        }

        /// <summary>
        ///     Finds if a given x and y indices are in the section.
        /// </summary>
        /// <param name="x">x-index</param>
        /// <param name="y">y-index</param>
        /// <returns>true if it is in the section, otherwise false</returns>
        private bool IsInSection(int x, int y)
        {
            if (x < SECTION_SIZE_X && y < SECTION_SIZE_Y && x >= 0 && y >= 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        ///     Finds if a given x and y indices are in the grid.
        /// </summary>
        /// <param name="x">x-index</param>
        /// <param name="y">y-index</param>
        /// <returns>true if it is in the grid, otherwise false</returns>
        private bool IsInGrid(int x, int y)
        {
            if (x < worldMaxX && y < worldMaxY && x >= 0 && y >= 0)
            {
                return true;
            }

            return false;
        }

        public Node GetNodeFromMousePosition()
        {
            Vector2 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Node node = GetNodeFromVector2(worldPosition);

            return node;
        }

        public Node GetNodeFromXY(int x, int y)
        {
            return grid[x, y];
        }

        public List<Node> GetNodesFromDimensions(Node middleNode, int xDim, int yDim)
        {
            List<Node> nodes = new List<Node>();
            int yStart = middleNode.y - yDim / 2;
            int xStart = middleNode.x - xDim / 2;

            for (int y = 0; y < yDim; y++)
            {
                for (int x = 0; x < xDim; x++)
                {
                    int nodeX = x + xStart;
                    int nodeY = y + yStart;
                    if (IsInSection(nodeX, nodeY))
                    {
                        nodes.Add(grid[nodeX, nodeY]);
                    }
                }
            }

            return nodes;
        }

      /*  private void OnDrawGizmos()
        {
            if (Application.isPlaying)
            {
                for (int y = 0; y < SECTION_SIZE_Y; y++)
                {
                    for (int x = 0; x < SECTION_SIZE_X; x++)
                    {
                        Gizmos.DrawSphere(grid[x, y].Position, 0.1f);
                        Gizmos.DrawWireCube(grid[x, y].Position, new Vector3(Node.NODE_DIAMETER, Node.NODE_DIAMETER, 0));
                    }
                }
            }
        }*/
    }
}

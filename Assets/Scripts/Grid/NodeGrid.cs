using FarmSim.Serialization;
using FarmSim.Utility;
using System.Collections.Generic;
using UnityEngine;

namespace FarmSim.Grid
{
    /// <class name="GridLayout">
    ///     <summary>
    ///         Contains a grid of <see cref="Node"/>'s and functions
    ///         that manage them.
    ///     </summary>
    /// </class>

    public class NodeGrid : Singleton<NodeGrid>, ISavable
    {
        [field: SerializeField] public int SectionNum { get; set; } = 0;

        public const int SECTION_SIZE_X = 30;
        public const int SECTION_SIZE_Y = 30;
        private readonly List<int> SaveableSections = new List<int> { 0 };

        private Node[,] grid;
        private SectionLoader sectionLoader = null;

        public bool LoadedSection { get; private set; } = false;

        private void Awake()
        {
            sectionLoader = new SectionLoader(transform.position, SectionNum, FindObjectOfType<ObjectPooler>());
            grid = sectionLoader.InitGrid();
            StartCoroutine(sectionLoader.LoadSection(grid, () => LoadedSection = true));
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
            int yStart = node.Data.y - yDim / 2;
            int xStart = node.Data.x - xDim / 2;

            for (int y = 0; y < yDim; y++)
            {
                for (int x = 0; x < xDim; x++)
                {
                    int nodeX = x + xStart;
                    int nodeY = y + yStart;
                    
                    // not valid placement if a node is outside the section or it is occupied
                    if (!IsInSection(nodeX, nodeY) || grid[nodeX, nodeY].Data.IsOccupied)
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
        public void MakeDimensionsOccupied(Node node, int xDim, int yDim, bool isWalkable = true)
        {
            int yStart = node.Data.y - yDim / 2;
            int xStart = node.Data.x - xDim / 2;

            for (int y = 0; y < yDim; y++)
            {
                for (int x = 0; x < xDim; x++)
                {
                    int nodeX = x + xStart;
                    int nodeY = y + yStart;
                    if (IsInSection(nodeX, nodeY))
                    {
                        grid[nodeX, nodeY].Data.IsOccupied = true;
                        grid[nodeX, nodeY].Data.IsWalkable = isWalkable;
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
            int yStart = middleNode.Data.y - yDim / 2;
            int xStart = middleNode.Data.x - xDim / 2;

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

        public List<Node> GetMooreNeighbours(Node middleNode)
        {
            List<Node> neighbours = new List<Node>();

            for (int y = -1; y <= 1; y++)
            {
                for (int x = -1; x <= 1; x++)
                {
                    if(x == 0 && y == 0)
                    {
                        continue;
                    }

                    int nodeX = middleNode.Data.x + x;
                    int nodeY = middleNode.Data.y + y;

                    if (IsInSection(nodeX, nodeY))
                    {
                        neighbours.Add(grid[nodeX, nodeY]);
                    }
                }
            }

            return neighbours;
        }

        public List<Node> GetCardinalNeighbours(Node middleNode)
        {
            List<Node> neighbours = new List<Node>();

            if (IsInSection(middleNode.Data.x - 1, middleNode.Data.y))
            {
                neighbours.Add(grid[middleNode.Data.x - 1, middleNode.Data.y]);
            }
            if (IsInSection(middleNode.Data.x + 1, middleNode.Data.y))
            {
                neighbours.Add(grid[middleNode.Data.x + 1, middleNode.Data.y]);
            }
            if (IsInSection(middleNode.Data.x, middleNode.Data.y - 1))
            {
                neighbours.Add(grid[middleNode.Data.x, middleNode.Data.y - 1]);
            }
            if (IsInSection(middleNode.Data.x, middleNode.Data.y + 1))
            {
                neighbours.Add(grid[middleNode.Data.x, middleNode.Data.y + 1]);
            }

            return neighbours;
        }

        public int GetManhattanDistance(Node node_1, Node node_2)
        {
            return Mathf.Abs(node_1.Data.x - node_2.Data.x) + Mathf.Abs(node_1.Data.y - node_2.Data.y);
        }

        public void Save()
        {
            if (!SaveableSections.Contains(SectionNum))
                return;

            SaveData.Current.nodeDatas[SectionNum] = new NodeData[grid.GetLength(0), grid.GetLength(1)];

            for (int x = 0; x < grid.GetLength(0); x++)
            {
                for (int y = 0; y < grid.GetLength(1); y++)
                {
                    SaveData.Current.nodeDatas[SectionNum][x, y] = grid[x, y].Data;
                }
            }
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

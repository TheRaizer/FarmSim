using FarmSim.Utility;
using System;
using System.Collections;
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
        /*[SerializeField] private float gridWorldX = 50;
        [SerializeField] private float gridWorldY = 50;*/

        [SerializeField] private int sectionNumber = 0;

        private int worldMaxX = 150;
        private int worldMaxY = 150;

        private int sectionXStart = 0;
        private int sectionYStart = 0;
        private int sectionXEnd = 0;
        private int sectionYEnd = 0;

        private const int SECTION_SIZE_X = 30;
        private const int SECTION_SIZE_Y = 30;

        private Node[,] grid;
        private bool loading = false;
        public bool LoadedSection { get; private set; } = false;

        private ObjectPooler pooler = null;

        private void Awake()
        {
            /*gridMaxX = Mathf.FloorToInt(gridWorldX / Node.NODE_DIAMETER);
            gridMaxY = Mathf.FloorToInt(gridWorldY / Node.NODE_DIAMETER);*/

            ExtractGridDimensionsFromTxt();
            CreateSection();
            if (worldMaxX % SECTION_SIZE_X != 0 || worldMaxY % SECTION_SIZE_Y != 0)
            {
                Debug.LogError("Section size does is not valid for grid dimensions");
            }

            grid = new Node[SECTION_SIZE_X, SECTION_SIZE_Y];
            InitSection();

            pooler = FindObjectOfType<ObjectPooler>();
        }

        private void Start()
        {
            StartCoroutine(LoadSection());
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

        /// <summary>
        /// Initialize's nodes into the <see cref="grid"/> and assigns a 
        /// world position to each.
        /// </summary>
        private void InitSection()
        {
            for (int y = 0; y < SECTION_SIZE_Y; y++)
            {
                for (int x = 0; x < SECTION_SIZE_X; x++)
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

        /// <summary>
        ///     Extracts a text file into a string array for each line.
        ///     It will use the first two lines to get the grids dimensions.
        /// </summary>
        private void ExtractGridDimensionsFromTxt()
        {
            string[] worldLines = File.ReadAllLines("C:/UnityProjects/FarmSim/Assets/Scripts/Grid/World.txt");
            try
            {
                worldMaxX = int.Parse(worldLines[0]);
                worldMaxY = int.Parse(worldLines[1]);


            }
            catch (FormatException)
            {
                Console.WriteLine("Unable to parse maxX or maxY");
                return;
            }
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

        private void CreateSection()
        {
            sectionXStart = sectionNumber * SECTION_SIZE_X;
            sectionXEnd = sectionXStart + SECTION_SIZE_X;

            sectionYStart = sectionNumber * SECTION_SIZE_Y;
            sectionYEnd = sectionYStart + SECTION_SIZE_Y;
        }

        private IEnumerator LoadSection()
        {
            if (!loading)
            {
                loading = true;
                string[] worldLines = File.ReadAllLines("C:/UnityProjects/FarmSim/Assets/Scripts/Grid/World.txt");

                // the + or - 2 when using sectionY is because the txt file worldLines has its first two lines as dimensions

                for (int y = sectionYStart + 2; y < sectionYEnd + 2; y++)
                {
                    string[] line = worldLines[y].Split(' ');

                    for (int x = sectionXStart; x < sectionXEnd; x++)
                    {
                        DetermineTileType(line[x], x - sectionXStart, y - sectionYStart - 2);
                        yield return null;
                    }
                }
                loading = false;
                LoadedSection = true;
            }
        }

        private void DetermineTileType(string val, int x, int y)
        {
            GameObject spawnedObject = null;
            switch (val)
            {
                case "0":
                    spawnedObject = pooler.SpawnGameObject("Dirt", grid[x, y].Position, Quaternion.identity);
                    break;
                default:
                    throw new ArgumentException($"No such tile for given code {val}");
            }

            if(spawnedObject.TryGetComponent(out IInteractable interactable))
            {
                grid[x, y].Interactable = interactable;
                interactable.X = x;
                interactable.Y = y;
            }
        }


        private void WriteToWorldFile()
        {
            using (StreamWriter file =
            new StreamWriter("C:/UnityProjects/FarmSim/Assets/Scripts/Grid/World.txt"))
            {
                file.WriteLine(worldMaxX);
                file.WriteLine(worldMaxY);
                for (int y = 0; y < worldMaxY; y++)
                {
                    for (int x = 0; x < worldMaxX; x++)
                    {
                        if (x != worldMaxX - 1)
                        {
                            file.Write("0 ");
                        }
                        else
                        {
                            file.Write("0\n");
                        }
                    }
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

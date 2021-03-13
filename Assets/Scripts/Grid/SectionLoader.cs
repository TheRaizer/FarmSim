using FarmSim.Serialization;
using FarmSim.Utility;
using System;
using System.Collections;
using System.IO;
using UnityEngine;

namespace FarmSim.Grid
{
    /// <class name="WorldLoader">
    ///     <summary>
    ///         Extracts the world and loads a section of the world.
    ///     </summary>
    /// </class>
    public class SectionLoader
    {
        private bool loading = false;

        private int worldMaxX = 150;
        private int worldMaxY = 150;

        private int sectionXStart = 0;
        private int sectionYStart = 0;
        private int sectionXEnd = 0;
        private int sectionYEnd = 0;

        private const int SECTION_SIZE_X = 30;
        private const int SECTION_SIZE_Y = 30;

        private readonly ObjectPooler pooler;
        private readonly Vector2 gridBottomLeft;
        private readonly int sectionNum = 0;

        public SectionLoader(Vector2 _gridBottomLeft, int _sectionNum, ObjectPooler _pooler)
        {
            gridBottomLeft = _gridBottomLeft;
            sectionNum = _sectionNum;
            pooler = _pooler;
        }

        public Node[,] InitGrid()
        {
            ExtractGridDimensionsFromTxt();
            CreateSection();
            if (worldMaxX % SECTION_SIZE_X != 0 || worldMaxY % SECTION_SIZE_Y != 0)
            {
                Debug.LogError("Section size does is not valid for grid dimensions");
            }

            Node[,] grid = new Node[SECTION_SIZE_X, SECTION_SIZE_Y];
            InitSection(grid);

            return grid;
        }

        /// <summary>
        /// Initialize's nodes into the <see cref="grid"/> and assigns a 
        /// world position to each.
        /// </summary>
        private void InitSection(Node[,] grid)
        {
            for (int y = 0; y < SECTION_SIZE_Y; y++)
            {
                for (int x = 0; x < SECTION_SIZE_X; x++)
                {
                    Vector2 pos = GetNodePosition(x, y);
                    grid[x, y] = new Node(new NodeData(false, pos, x, y));
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

            Vector2 pos = new Vector2(xPos + gridBottomLeft.x, yPos + gridBottomLeft.y);
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
        private void CreateSection()
        {
            sectionXStart = sectionNum * SECTION_SIZE_X;
            sectionXEnd = sectionXStart + SECTION_SIZE_X;

            sectionYStart = sectionNum * SECTION_SIZE_Y;
            sectionYEnd = sectionYStart + SECTION_SIZE_Y;
        }

        public IEnumerator LoadSection(Node[,] grid, Action onLoaded = null)
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
                        DetermineTileType(line[x], x - sectionXStart, y - sectionYStart - 2, grid);
                        yield return null;
                    }
                }
                loading = false;
                onLoaded?.Invoke();
            }
        }

        private void DetermineTileType(string val, int x, int y, Node[,] grid)
        {
            GameObject spawnedObject = null;
            switch (val)
            {
                case "0":
                    spawnedObject = pooler.SpawnGameObject("Dirt", grid[x, y].Data.pos, Quaternion.identity);
                    break;
                default:
                    throw new ArgumentException($"No such tile for given code {val}");
            }

            if (spawnedObject.TryGetComponent(out IInteractable interactable))
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
    }
}

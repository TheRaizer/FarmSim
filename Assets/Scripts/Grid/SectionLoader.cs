﻿using FarmSim.SavableData;
using FarmSim.Utility;
using System;
using System.Collections;
using System.IO;
using UnityEngine;

namespace FarmSim.Grid
{
    /// <class name="SectionLoader">
    ///     <summary>
    ///         Extracts the world and loads a section of the world.
    ///     </summary>
    /// </class>
    public class SectionLoader
    {
        private bool loading = false;

        public int WorldMaxX { get; private set; } = 150;
        public int WorldMaxY { get; private set; } = 150;

        private int sectionXStart = 0;
        private int sectionYStart = 0;
        private int sectionXEnd = 0;
        private int sectionYEnd = 0;

        public const int SECTION_SIZE_X = 30;
        public const int SECTION_SIZE_Y = 30;

        private readonly ObjectPooler pooler;
        private readonly Vector2 gridBottomLeft;
        private readonly int sectionNum = 0;

        private bool SectionNotSaved => SectionData.Current.NodeDatas == null || SectionData.Current.NodeDatas.Length <= 0;

        public SectionLoader(Vector2 _gridBottomLeft, int _sectionNum, ObjectPooler _pooler)
        {
            gridBottomLeft = _gridBottomLeft;
            sectionNum = _sectionNum;
            pooler = _pooler;
        }

        public Node[,] InitGrid()
        {
            ExtractGridDimensionsFromTxt();
            Node[,] sectionGrid = new Node[SECTION_SIZE_X, SECTION_SIZE_Y];

            return sectionGrid;
        }

        /// <summary>
        /// Initialize's nodes into the <see cref="grid"/> and assigns a 
        /// world position to each.
        /// </summary>
        public void InitSection(Node[,] sectionGrid)
        {
            CreateSectionBounds();

            if (WorldMaxX % SECTION_SIZE_X != 0 || WorldMaxY % SECTION_SIZE_Y != 0)
                Debug.LogError("Section size does is not valid for grid dimensions");
            if (sectionNum < 0 || sectionNum * SECTION_SIZE_X >= WorldMaxX || sectionNum * SECTION_SIZE_Y >= WorldMaxY)
                Debug.LogError($"Section number {sectionNum} is not valid");



            for (int y = 0; y < SECTION_SIZE_Y; y++)
            {
                for (int x = 0; x < SECTION_SIZE_X; x++)
                {
                    // if there are no nodes that have been saved for this section
                    if (SectionNotSaved)
                    {
                        // create new ones
                        Vector2 pos = GetNodePosition(x, y);
                        sectionGrid[x, y] = new Node(new NodeData(false, pos, x, y));
                    }
                    else
                    {
                        // load saved ones
                        sectionGrid[x, y] = new Node(SectionData.Current.NodeDatas[x, y]);
                    }
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
            float xPos = x * NodeMeasures.NODE_DIAMETER + NodeMeasures.NODE_RADIUS;
            float yPos = y * NodeMeasures.NODE_DIAMETER + NodeMeasures.NODE_RADIUS;

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
                WorldMaxX = int.Parse(worldLines[0]);
                WorldMaxY = int.Parse(worldLines[1]);
            }
            catch (FormatException)
            {
                Console.WriteLine("Unable to parse maxX or maxY");
                return;
            }
        }

        private void CreateSectionBounds()
        {
            sectionXStart = sectionNum * SECTION_SIZE_X;
            sectionXEnd = sectionXStart + SECTION_SIZE_X;

            sectionYStart = sectionNum * SECTION_SIZE_Y;
            sectionYEnd = sectionYStart + SECTION_SIZE_Y;
        }

        /// <summary>
        ///     Loads the sections tiles on another thread.
        /// </summary>
        /// <param name="sectionGrid">The <see cref="Node"/>'s of the section</param>
        /// <param name="onLoaded">Action to run once loading is finished</param>
        public IEnumerator LoadSectionCo(Node[,] sectionGrid, Action onLoaded = null)
        {
            if (!loading)
            {
                loading = true;
                string[] worldLines = File.ReadAllLines("C:/UnityProjects/FarmSim/Assets/Scripts/Grid/World.txt");

                // the + or - 2 when using sectionY is because the txt file worldLines has its first two lines as dimensions

                for (int y = sectionYStart + 2; y < sectionYEnd + 2; y++)
                {
                    string[] line = worldLines[y].Split(' ');
                    yield return null;

                    for (int x = sectionXStart; x < sectionXEnd; x++)
                    {
                        DetermineTileType(line[x], x - sectionXStart, y - sectionYStart - 2, sectionGrid);
                    }
                }
                loading = false;
                onLoaded?.Invoke();
            }
            Debug.Log("rendered section");
        }

        /// <summary>
        ///     Spawns a given tile at the proper position, initializing the node data's fields when necessary.
        /// </summary>
        /// <param name="val">The tile type as read from the world's txt file</param>
        /// <param name="x">x idx of the node</param>
        /// <param name="y">y idx of the node</param>
        /// <param name="sectionGrid"></param>
        private void DetermineTileType(string val, int x, int y, Node[,] sectionGrid)
        {
            if (pooler == null)
                return;
            GameObject spawnedObject;
            switch (val)
            {
                case "0":
                    if (SectionNotSaved)
                    {
                        // if the section isnt saved then initialize the nodes default fields.
                        sectionGrid[x, y].Data.IsWalkable = true;
                        sectionGrid[x, y].Data.IsOccupied = false;
                    }
                    spawnedObject = pooler.SpawnGameObject("Dirt", sectionGrid[x, y].Data.pos, Quaternion.identity);
                    break;
                case "1":
                    sectionGrid[x, y].Data.IsWalkable = false;
                    sectionGrid[x, y].Data.IsOccupied = true;
                    spawnedObject = pooler.SpawnGameObject("Water", sectionGrid[x, y].Data.pos, Quaternion.identity);
                    break;
                default:
                    throw new ArgumentException($"No such tile for given code {val}");
            }

            // if the spawned object is an IInteractable, assign its x and y and reference it in the corrosponding node 
            if (spawnedObject.TryGetComponent(out IInteractable interactable))
            {
                sectionGrid[x, y].Interactable = interactable;
                interactable.X = x;
                interactable.Y = y;
            }
        }

        private void WriteToWorldFile()
        {
            using (StreamWriter file =
            new StreamWriter("C:/UnityProjects/FarmSim/Assets/Scripts/Grid/World.txt"))
            {
                file.WriteLine(WorldMaxX);
                file.WriteLine(WorldMaxY);
                for (int y = 0; y < WorldMaxY; y++)
                {
                    for (int x = 0; x < WorldMaxX; x++)
                    {
                        if (x != WorldMaxX - 1)
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

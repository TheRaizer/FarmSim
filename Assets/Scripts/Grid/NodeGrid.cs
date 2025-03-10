﻿using FarmSim.Attributes;
using FarmSim.SavableData;
using FarmSim.Serialization;
using FarmSim.Utility;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FarmSim.Grid
{
    /// <class name="NodeGrid">
    ///     <summary>
    ///         Contains a grid of <see cref="Node"/>'s and functions
    ///         that manage them.
    ///     </summary>
    /// </class>

    [Savable(false)]
    public class NodeGrid : MonoBehaviour, ISavable
    {
        public int SectionNum { get; set; } = 0;

        public const int SECTION_SIZE_X = 30;
        public const int SECTION_SIZE_Y = 30;
        private readonly List<int> SaveableSections = new List<int> { 0, 2 };

        private Node[,] sectionGrid;
        private SectionLoader sectionLoader = null;

        public bool LoadedSection { get; private set; } = false;
        public bool IsSavableSection => SaveableSections.Contains(SectionNum);

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);

            SectionNum = PlayerData.Current.SectionNum;
            SceneManager.sceneLoaded += LoadSectionOnSceneLoad;
        }



        /// <summary>
        ///     Attempts to load a section on scene change
        /// </summary>
        /// <param name="scene">The scene being loaded</param>
        /// <param name="mode">The mode the scene is being loaded</param>
        private void LoadSectionOnSceneLoad(Scene scene, LoadSceneMode mode)
        {
            LoadedSection = false;

            // if the scene is a test scene the section # = 0 otherwise it is always scene index - 1.
            SectionNum = scene.buildIndex == -1 ? 0 : scene.buildIndex - 1;

            sectionLoader = new SectionLoader(transform.position, SectionNum, FindObjectOfType<ObjectPooler>());

            // initialize an empty grid.
            sectionGrid = sectionLoader.InitGrid();

            // if the scene is not a section scene as per the scene to section rule
            if (scene.buildIndex == 0 || scene.buildIndex - 1 >= sectionLoader.WorldMaxX / SECTION_SIZE_X)
            {
                // if the scene does not need loading don't load.
                Debug.Log("no need to load this sections grid.");
                return;
            }

            // init the section and its GameObjects.
            sectionLoader.InitSection(sectionGrid);
            StartCoroutine(sectionLoader.LoadSectionCo(sectionGrid, () => LoadedSection = true));
        }

        /// <summary>
        ///     Unit Test function
        /// </summary>
        public void LoadSectionTest(int sectionNum = 0)
        {
            LoadedSection = false;

            // section # is always 1 less then the scene index.
            SectionNum = sectionNum;

            sectionLoader = new SectionLoader(transform.position, SectionNum, FindObjectOfType<ObjectPooler>());

            // initialize an empty grid.
            sectionGrid = sectionLoader.InitGrid();

            // init the section and its GameObjects.
            sectionLoader.InitSection(sectionGrid);
            StartCoroutine(sectionLoader.LoadSectionCo(sectionGrid, () => LoadedSection = true));
            //sectionLoader.LoadSectionVoid(sectionGrid, () => LoadedSection = true);
        }

        /// <summary>
        ///     Obtains a <see cref="Node"/> from the <see cref="sectionGrid"/> given a <see cref="Vector2"/>.
        /// </summary>
        /// <param name="vector">The vector to find a Node from</param>
        /// <returns>
        ///     <see cref="Node"/> that is located in the given <see cref="Vector2"/>.
        ///     Null if vector is not part of a node.
        /// </returns>
        public Node GetNodeFromVector2(Vector2 vector)
        {
            // round the actual value to 4 decimal places to avoid incorrect indexes
            float xVal = (float)Math.Round((float)((vector.x - transform.position.x) / NodeMeasures.NODE_DIAMETER), 4);
            float yVal = (float)Math.Round((float)((vector.y - transform.position.y) / NodeMeasures.NODE_DIAMETER), 4);

            // floor the values to get the correct node in the case that the vector is not exactly at the nodes position
            int x = Mathf.FloorToInt(xVal);
            int y = Mathf.FloorToInt(yVal);

            if (IsInSection(x, y))
            {
                return sectionGrid[x, y];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        ///     Checks every node in a certain space around a given node and returns if something can be placed.
        /// </summary>
        /// <param name="nodeData">The node whose at the center of the dimensions.</param>
        /// <param name="xDim">The x-dimension to check.</param>
        /// <param name="yDim">The y-dimension to check.</param>
        /// <returns>true if there are no occupied Nodes in the space, otherwise false.</returns>
        public bool IsValidPlacement(INodeData nodeData, int xDim, int yDim)
        {
            int yStart = nodeData.Data.y - (yDim / 2);
            int xStart = nodeData.Data.x - (xDim / 2);

            for (int y = 0; y < yDim; y++)
            {
                for (int x = 0; x < xDim; x++)
                {
                    int nodeX = x + xStart;
                    int nodeY = y + yStart;

                    // not valid placement if a node is outside the section or it is occupied
                    if (!IsInSection(nodeX, nodeY) || sectionGrid[nodeX, nodeY].Data.IsOccupied)
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
        /// <param name="nodeData">The node the dimensions surround.</param>
        /// <param name="xDim">The x-dimension.</param>
        /// <param name="yDim">The y-dimension.</param>
        public void MakeDimensionsOccupied(INodeData nodeData, int xDim, int yDim, bool isWalkable = true)
        {
            int yStart = nodeData.Data.y - (yDim / 2);
            int xStart = nodeData.Data.x - (xDim / 2);

            for (int y = 0; y < yDim; y++)
            {
                for (int x = 0; x < xDim; x++)
                {
                    int nodeX = x + xStart;
                    int nodeY = y + yStart;
                    if (IsInSection(nodeX, nodeY))
                    {
                        sectionGrid[nodeX, nodeY].Data.IsOccupied = true;
                        sectionGrid[nodeX, nodeY].Data.IsWalkable = isWalkable;
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
            if (IsInSection(x, y))
                return sectionGrid[x, y];
            else
                return null;
        }

        public List<Node> GetNodesFromDimensions(INodeData middleNode, int xDim, int yDim)
        {
            List<Node> nodes = new List<Node>();
            int yStart = middleNode.Data.y - (yDim / 2);
            int xStart = middleNode.Data.x - (xDim / 2);

            for (int y = 0; y < yDim; y++)
            {
                for (int x = 0; x < xDim; x++)
                {
                    int nodeX = x + xStart;
                    int nodeY = y + yStart;
                    if (IsInSection(nodeX, nodeY))
                    {
                        nodes.Add(sectionGrid[nodeX, nodeY]);
                    }
                }
            }

            return nodes;
        }

        public List<Node> GetMooreNeighbours(INodeData middleNode)
        {
            List<Node> neighbours = new List<Node>();

            for (int y = -1; y <= 1; y++)
            {
                for (int x = -1; x <= 1; x++)
                {
                    if (x == 0 && y == 0)
                    {
                        continue;
                    }

                    int nodeX = middleNode.Data.x + x;
                    int nodeY = middleNode.Data.y + y;

                    if (IsInSection(nodeX, nodeY))
                    {
                        neighbours.Add(sectionGrid[nodeX, nodeY]);
                    }
                }
            }

            return neighbours;
        }

        public List<Node> GetCardinalNeighbours(INodeData middleNode)
        {
            List<Node> neighbours = new List<Node>();

            if (IsInSection(middleNode.Data.x - 1, middleNode.Data.y))
            {
                neighbours.Add(sectionGrid[middleNode.Data.x - 1, middleNode.Data.y]);
            }
            if (IsInSection(middleNode.Data.x + 1, middleNode.Data.y))
            {
                neighbours.Add(sectionGrid[middleNode.Data.x + 1, middleNode.Data.y]);
            }
            if (IsInSection(middleNode.Data.x, middleNode.Data.y - 1))
            {
                neighbours.Add(sectionGrid[middleNode.Data.x, middleNode.Data.y - 1]);
            }
            if (IsInSection(middleNode.Data.x, middleNode.Data.y + 1))
            {
                neighbours.Add(sectionGrid[middleNode.Data.x, middleNode.Data.y + 1]);
            }

            return neighbours;
        }

        public int GetManhattanDistance(INodeData node_1, INodeData node_2)
        {
            return Mathf.Abs(node_1.Data.x - node_2.Data.x) + Mathf.Abs(node_1.Data.y - node_2.Data.y);
        }

        public void Save()
        {
            PlayerData.Current.SectionNum = SectionNum;

            SectionData.Current.SectionNum = SectionNum;
            SectionData.Current.NodeDatas = new NodeData[sectionGrid.GetLength(0), sectionGrid.GetLength(1)];

            for (int x = 0; x < sectionGrid.GetLength(0); x++)
            {
                for (int y = 0; y < sectionGrid.GetLength(1); y++)
                {
                    SectionData.Current.NodeDatas[x, y] = sectionGrid[x, y].Data;
                }
            }
        }
    }
}

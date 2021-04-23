using FarmSim.Grid;
using FarmSim.Serialization;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

namespace Tests
{
    public class NodeGridTests
    {
        private NodeGrid currNodeGrid;

        [OneTimeSetUp]
        public void SetUp()
        {
            currNodeGrid = new GameObject().AddComponent<NodeGrid>();

            // all tests should work even if the location of the section is changed
            currNodeGrid.transform.position = new Vector2(Random.Range(-100, 100), Random.Range(-100, 100));
            currNodeGrid.LoadSectionTest();
        }

        [Test]
        public void GetNodeFromVector2Test()
        {
            // get a node
            Node node_1 = currNodeGrid.GetNodeFromVector2(new Vector2(Node.NODE_DIAMETER - 0.05f, Node.NODE_DIAMETER - 0.05f)
                + (Vector2)currNodeGrid.transform.position);

            // make sure the node obtained is the correct node
            Assert.AreEqual(0, node_1.Data.x);
            Assert.AreEqual(0, node_1.Data.y);

            Node node_2 = currNodeGrid.GetNodeFromVector2(new Vector2(Node.NODE_DIAMETER * 6 - 0.05f, Node.NODE_DIAMETER * 8 - 0.05f)
                + (Vector2)currNodeGrid.transform.position);

            Assert.AreEqual(5, node_2.Data.x);
            Assert.AreEqual(7, node_2.Data.y);

            Node node_3 = currNodeGrid.GetNodeFromVector2(new Vector2(Node.NODE_DIAMETER * 6 + 0.05f, Node.NODE_DIAMETER * 8 + 0.05f)
                + (Vector2)currNodeGrid.transform.position);

            Assert.AreEqual(6, node_3.Data.x);
            Assert.AreEqual(8, node_3.Data.y);

            Node node_4 = currNodeGrid.GetNodeFromVector2(new Vector2(Node.NODE_DIAMETER * 25, Node.NODE_DIAMETER * 22)
                + (Vector2)currNodeGrid.transform.position);

            Assert.AreEqual(25, node_4.Data.x);
            Assert.AreEqual(22, node_4.Data.y);
        }

        [Test]
        public void IsValidPlacementTest()
        {
            Node node = currNodeGrid.GetNodeFromVector2(new Vector2(Node.NODE_DIAMETER * 5, Node.NODE_DIAMETER * 5)
                + (Vector2)currNodeGrid.transform.position);

            currNodeGrid.MakeDimensionsOccupied(node, 3, 3);

            List<Node> neighbours = currNodeGrid.GetMooreNeighbours(node);

            neighbours.ForEach(x => Assert.IsFalse(currNodeGrid.IsValidPlacement(x, 1, 1)));

            Assert.IsFalse(currNodeGrid.IsValidPlacement(node, 1, 1));
        }

        [Test]
        public void MakeDimensionsOccupiedTest()
        {
            var grid = Object.FindObjectOfType<NodeGrid>();

            Node node = grid.GetNodeFromVector2(new Vector2(Node.NODE_DIAMETER * 5, Node.NODE_DIAMETER * 5)
                + (Vector2)grid.transform.position);

            grid.MakeDimensionsOccupied(node, 3, 3);

            List<Node> neighbours = grid.GetMooreNeighbours(node);

            neighbours.ForEach(x => Assert.IsTrue(x.Data.IsOccupied));

            Assert.IsTrue(node.Data.IsOccupied);
        }

        [Test]
        public void MooreNeighboursTest()
        {
            Node node = currNodeGrid.GetNodeFromVector2(new Vector2(Node.NODE_DIAMETER * 5, Node.NODE_DIAMETER * 5)
                + (Vector2)currNodeGrid.transform.position);

            List<Node> neighbours = currNodeGrid.GetMooreNeighbours(node);

            Assert.IsTrue(neighbours.Count == 8);

            // manually check that each neighbour has the correct x and y coords
            Assert.IsTrue(neighbours[0].Data.x == node.Data.x - 1 && neighbours[0].Data.y == node.Data.y - 1);
            Assert.IsTrue(neighbours[1].Data.x == node.Data.x && neighbours[1].Data.y == node.Data.y - 1);
            Assert.IsTrue(neighbours[2].Data.x == node.Data.x + 1 && neighbours[2].Data.y == node.Data.y - 1);

            Assert.IsTrue(neighbours[3].Data.x == node.Data.x - 1 && neighbours[3].Data.y == node.Data.y);

            // do not check the exact node coords as it is not a neighbours

            Assert.IsTrue(neighbours[4].Data.x == node.Data.x + 1 && neighbours[4].Data.y == node.Data.y);

            Assert.IsTrue(neighbours[5].Data.x == node.Data.x - 1 && neighbours[5].Data.y == node.Data.y + 1);
            Assert.IsTrue(neighbours[6].Data.x == node.Data.x && neighbours[6].Data.y == node.Data.y + 1);
            Assert.IsTrue(neighbours[7].Data.x == node.Data.x + 1 && neighbours[7].Data.y == node.Data.y + 1);
        }

        [Test]
        public void CardinalNeighboursTest()
        {
            Node node = currNodeGrid.GetNodeFromVector2(new Vector2(Node.NODE_DIAMETER * 5, Node.NODE_DIAMETER * 5)
                + (Vector2)currNodeGrid.transform.position);

            List<Node> neighbours = currNodeGrid.GetCardinalNeighbours(node);

            Assert.IsTrue(neighbours.Count == 4);

            // manually check that each neighbour has the correct x and y coords
            Assert.IsTrue(neighbours[0].Data.x == node.Data.x - 1 && neighbours[0].Data.y == node.Data.y);
            Assert.IsTrue(neighbours[1].Data.x == node.Data.x + 1 && neighbours[1].Data.y == node.Data.y);
            Assert.IsTrue(neighbours[2].Data.x == node.Data.x && neighbours[2].Data.y == node.Data.y - 1);
            Assert.IsTrue(neighbours[3].Data.x == node.Data.x && neighbours[3].Data.y == node.Data.y + 1);
        }

        [Test]
        public void ManhattanDistanceTest()
        {
            Vector2 vector_1 = new Vector2(Node.NODE_DIAMETER * 5, Node.NODE_DIAMETER * 5);
            Vector2 vector_2 = new Vector2(Node.NODE_DIAMETER * 6, Node.NODE_DIAMETER * 6);
            Vector2 vector_3 = new Vector2(Node.NODE_DIAMETER * 5, Node.NODE_DIAMETER * 9);

            Node node_1 = currNodeGrid.GetNodeFromVector2(vector_1 + (Vector2)currNodeGrid.transform.position);
            Node node_2 = currNodeGrid.GetNodeFromVector2(vector_2 + (Vector2)currNodeGrid.transform.position);
            Node node_3 = currNodeGrid.GetNodeFromVector2(vector_3 + (Vector2)currNodeGrid.transform.position);

            Assert.AreEqual(2, currNodeGrid.GetManhattanDistance(node_1, node_2));
            Assert.AreEqual(2, currNodeGrid.GetManhattanDistance(node_2, node_1));

            Assert.AreEqual(4, currNodeGrid.GetManhattanDistance(node_1, node_3));
            Assert.AreEqual(4, currNodeGrid.GetManhattanDistance(node_3, node_1));

            Assert.AreEqual(4, currNodeGrid.GetManhattanDistance(node_3, node_2));
            Assert.AreEqual(4, currNodeGrid.GetManhattanDistance(node_2, node_3));
        }

        [Test]
        public void GridSaveTest()
        {
            currNodeGrid.LoadSectionTest(1);
            currNodeGrid.Save();

            Assert.AreEqual(1, PlayerData.Current.SectionNum);
            Assert.AreEqual(1, SectionData.Current.SectionNum);

            Assert.AreEqual(30, SectionData.Current.nodeDatas.GetLength(0));
            Assert.AreEqual(30, SectionData.Current.nodeDatas.GetLength(1));

            for (int x = 0; x < SectionData.Current.nodeDatas.GetLength(0); x++)
            {
                for (int y = 0; y < SectionData.Current.nodeDatas.GetLength(1); y++)
                {
                    Assert.NotNull(SectionData.Current.nodeDatas[x, y]);
                }
            }
        }
    }
}

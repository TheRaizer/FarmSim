﻿using FarmSim.Grid;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Tests
{
    public class NodeGridTests
    {
        [UnitySetUp]
        public void SetUp()
        {
            var obj = new GameObject().AddComponent<NodeGrid>().gameObject;
            Object.Instantiate(obj);
        }

        [UnityTearDown]
        public void TearDown()
        {
            Object.Destroy(Object.FindObjectOfType<NodeGrid>());
        }

        [Test]
        public void GetNodeFromVector2Test()
        {
            var grid = Object.FindObjectOfType<NodeGrid>();

            // get a node
            Node node_1 = grid.GetNodeFromVector2(new Vector2(Node.NODE_DIAMETER - 0.05f, Node.NODE_DIAMETER - 0.05f) + (Vector2)grid.transform.position);

            // make sure the node obtained is the correct node
            Assert.AreEqual(0, node_1.Data.x);
            Assert.AreEqual(0, node_1.Data.y);

            Node node_2 = grid.GetNodeFromVector2(new Vector2(Node.NODE_DIAMETER * 6 - 0.05f, Node.NODE_DIAMETER * 8 - 0.05f) + (Vector2)grid.transform.position);

            Assert.AreEqual(5, node_2.Data.x);
            Assert.AreEqual(7, node_2.Data.y);

            Node node_3 = grid.GetNodeFromVector2(new Vector2(Node.NODE_DIAMETER * 6 + 0.05f, Node.NODE_DIAMETER * 8 + 0.05f) + (Vector2)grid.transform.position);

            Assert.AreEqual(6, node_3.Data.x);
            Assert.AreEqual(8, node_3.Data.y);

            Node node_4 = grid.GetNodeFromVector2(new Vector2(Node.NODE_DIAMETER * 25, Node.NODE_DIAMETER * 22) + (Vector2)grid.transform.position);

            Assert.AreEqual(25, node_4.Data.x);
            Assert.AreEqual(22, node_4.Data.y);
        }

        [Test]
        public void IsValidPlacementTest()
        {
            var grid = Object.FindObjectOfType<NodeGrid>();

            Node node = grid.GetNodeFromVector2(new Vector2(Node.NODE_DIAMETER * 5, Node.NODE_DIAMETER * 5) + (Vector2)grid.transform.position);

            grid.MakeDimensionsOccupied(node, 3, 3);

            List<Node> neighbours = grid.GetMooreNeighbours(node);

            neighbours.ForEach(x => Assert.IsFalse(grid.IsValidPlacement(x, 1, 1)));

            Assert.IsFalse(grid.IsValidPlacement(node, 1, 1));
        }

        [Test]
        public void MakeDimensionsOccupiedTest()
        {
            var grid = Object.FindObjectOfType<NodeGrid>();

            Node node = grid.GetNodeFromVector2(new Vector2(Node.NODE_DIAMETER * 5, Node.NODE_DIAMETER * 5) + (Vector2)grid.transform.position);

            grid.MakeDimensionsOccupied(node, 3, 3);

            List<Node> neighbours = grid.GetMooreNeighbours(node);

            neighbours.ForEach(x => Assert.IsTrue(x.Data.IsOccupied));

            Assert.IsTrue(node.Data.IsOccupied);
        }

        [Test]
        public void MooreNeighboursTest()
        {
            var grid = Object.FindObjectOfType<NodeGrid>();

            Node node = grid.GetNodeFromVector2(new Vector2(Node.NODE_DIAMETER * 5, Node.NODE_DIAMETER * 5) + (Vector2)grid.transform.position);

            List<Node> neighbours = grid.GetMooreNeighbours(node);

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
            var grid = Object.FindObjectOfType<NodeGrid>();

            Node node = grid.GetNodeFromVector2(new Vector2(Node.NODE_DIAMETER * 5, Node.NODE_DIAMETER * 5) + (Vector2)grid.transform.position);

            List<Node> neighbours = grid.GetCardinalNeighbours(node);

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
            var grid = Object.FindObjectOfType<NodeGrid>();

            Node node_1 = grid.GetNodeFromVector2(new Vector2(Node.NODE_DIAMETER * 5, Node.NODE_DIAMETER * 5) + (Vector2)grid.transform.position);
            Node node_2 = grid.GetNodeFromVector2(new Vector2(Node.NODE_DIAMETER * 6, Node.NODE_DIAMETER * 6) + (Vector2)grid.transform.position);
            Node node_3 = grid.GetNodeFromVector2(new Vector2(Node.NODE_DIAMETER * 5, Node.NODE_DIAMETER * 9) + (Vector2)grid.transform.position);

            Assert.IsTrue(grid.GetManhattanDistance(node_1, node_2) == 2);
            Assert.IsTrue(grid.GetManhattanDistance(node_2, node_1) == 2);

            Assert.IsTrue(grid.GetManhattanDistance(node_1, node_3) == 4);
            Assert.IsTrue(grid.GetManhattanDistance(node_3, node_1) == 4);

            Assert.IsTrue(grid.GetManhattanDistance(node_3, node_2) == 4);
            Assert.IsTrue(grid.GetManhattanDistance(node_2, node_3) == 4);
        }
    }
}

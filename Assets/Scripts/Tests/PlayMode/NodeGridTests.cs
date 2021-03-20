using NUnit.Framework;
using UnityEngine;
using FarmSim.Grid;
using System.Collections.Generic;

namespace Tests
{
    public class NodeGridTests
    {
        [Test]
        public void GetNodeFromVector2Test()
        {

            var gameObject = new GameObject();
            var grid = gameObject.AddComponent<NodeGrid>();
            grid.transform.position = new Vector3(55, -25.4f, 0);

            Node node_1 = grid.GetNodeFromVector2(new Vector2(Node.NODE_DIAMETER - 0.05f, Node.NODE_DIAMETER - 0.05f) + (Vector2)gameObject.transform.position);

            Assert.AreEqual(0, node_1.Data.x);
            Assert.AreEqual(0, node_1.Data.y);

            Node node_2 = grid.GetNodeFromVector2(new Vector2(Node.NODE_DIAMETER * 6 - 0.05f, Node.NODE_DIAMETER * 8 - 0.05f) + (Vector2)gameObject.transform.position);

            Assert.AreEqual(5, node_2.Data.x);
            Assert.AreEqual(7, node_2.Data.y);

            Node node_3 = grid.GetNodeFromVector2(new Vector2(Node.NODE_DIAMETER * 6 + 0.05f, Node.NODE_DIAMETER * 8 + 0.05f) + (Vector2)gameObject.transform.position);

            Assert.AreEqual(6, node_3.Data.x);
            Assert.AreEqual(8, node_3.Data.y);

            Node node_4 = grid.GetNodeFromVector2(new Vector2(Node.NODE_DIAMETER * 25, Node.NODE_DIAMETER * 22) + (Vector2)gameObject.transform.position);

            Assert.AreEqual(25, node_4.Data.x);
            Assert.AreEqual(22, node_4.Data.y);
        }

        [Test]
        public void IsValidPlacementTest()
        {
            var gameObject = new GameObject();
            var grid = gameObject.AddComponent<NodeGrid>();
            grid.transform.position = Vector3.zero;

            Node node = grid.GetNodeFromVector2(new Vector2(Node.NODE_DIAMETER * 5, Node.NODE_DIAMETER * 5));

            grid.MakeDimensionsOccupied(node, 3, 3);

            List<Node> neighbours = grid.GetMooreNeighbours(node);

            neighbours.ForEach(x => Assert.IsFalse(grid.IsValidPlacement(x, 1, 1)));

            Assert.IsFalse(grid.IsValidPlacement(node, 1, 1));
        }
        
        [Test]
        public void MakeDimensionsOccupiedTest()
        {
            var gameObject = new GameObject();
            var grid = gameObject.AddComponent<NodeGrid>();
            grid.transform.position = Vector3.zero;

            Node node = grid.GetNodeFromVector2(new Vector2(Node.NODE_DIAMETER * 5, Node.NODE_DIAMETER * 5));

            grid.MakeDimensionsOccupied(node, 3, 3);

            List<Node> neighbours = grid.GetMooreNeighbours(node);

            neighbours.ForEach(x => Assert.IsTrue(x.Data.IsOccupied));

            Assert.IsTrue(node.Data.IsOccupied);
        }
    }
}

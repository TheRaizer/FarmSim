using NUnit.Framework;
using UnityEngine;
using FarmSim.Grid;

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

            Assert.AreEqual(0, node_1.x);
            Assert.AreEqual(0, node_1.y);

            Node node_2 = grid.GetNodeFromVector2(new Vector2(Node.NODE_DIAMETER * 6 - 0.05f, Node.NODE_DIAMETER * 8 - 0.05f) + (Vector2)gameObject.transform.position);

            Assert.AreEqual(5, node_2.x);
            Assert.AreEqual(7, node_2.y);

            Node node_3 = grid.GetNodeFromVector2(new Vector2(Node.NODE_DIAMETER * 6 + 0.05f, Node.NODE_DIAMETER * 8 + 0.05f) + (Vector2)gameObject.transform.position);

            Assert.AreEqual(6, node_3.x);
            Assert.AreEqual(8, node_3.y);

            Node node_4 = grid.GetNodeFromVector2(new Vector2(Node.NODE_DIAMETER * 25, Node.NODE_DIAMETER * 22) + (Vector2)gameObject.transform.position);

            Assert.AreEqual(25, node_4.x);
            Assert.AreEqual(22, node_4.y);
        }

        [Test]
        public void IsValidPlacementTest()
        {
            var gameObject = new GameObject();
            var grid = gameObject.AddComponent<NodeGrid>();
            grid.transform.position = Vector3.zero;

            Node node = grid.GetNodeFromVector2(new Vector2(Node.NODE_DIAMETER * 40, Node.NODE_DIAMETER * 40));

            grid.MakeDimensionsOccupied(node, 3, 3);

            Node nodeTopLeft = grid.GetNodeFromVector2(new Vector2(Node.NODE_DIAMETER * 39, Node.NODE_DIAMETER * 41));
            Node nodeTopMid = grid.GetNodeFromVector2(new Vector2(Node.NODE_DIAMETER * 40, Node.NODE_DIAMETER * 41));
            Node nodeTopRight = grid.GetNodeFromVector2(new Vector2(Node.NODE_DIAMETER * 41, Node.NODE_DIAMETER * 41));

            Node nodeLeft = grid.GetNodeFromVector2(new Vector2(Node.NODE_DIAMETER * 39, Node.NODE_DIAMETER * 40));
            Node nodeRight = grid.GetNodeFromVector2(new Vector2(Node.NODE_DIAMETER * 41, Node.NODE_DIAMETER * 40));

            Node nodeBottomLeft = grid.GetNodeFromVector2(new Vector2(Node.NODE_DIAMETER * 39, Node.NODE_DIAMETER * 39));
            Node nodeBottomMid = grid.GetNodeFromVector2(new Vector2(Node.NODE_DIAMETER * 40, Node.NODE_DIAMETER * 39));
            Node nodeBottomRight = grid.GetNodeFromVector2(new Vector2(Node.NODE_DIAMETER * 41, Node.NODE_DIAMETER * 39));

            Assert.IsFalse(grid.IsValidPlacement(nodeTopLeft, 1, 1));
            Assert.IsFalse(grid.IsValidPlacement(nodeTopMid, 1, 1));
            Assert.IsFalse(grid.IsValidPlacement(nodeTopRight, 1, 1));

            Assert.IsFalse(grid.IsValidPlacement(nodeLeft, 1, 1));
            Assert.IsFalse(grid.IsValidPlacement(nodeRight, 1, 1));

            Assert.IsFalse(grid.IsValidPlacement(nodeBottomLeft, 1, 1));
            Assert.IsFalse(grid.IsValidPlacement(nodeBottomMid, 1, 1));
            Assert.IsFalse(grid.IsValidPlacement(nodeBottomRight, 1, 1));

            Assert.IsFalse(grid.IsValidPlacement(node, 1, 1));
        }
        
        [Test]
        public void MakeDimensionsOccupiedTest()
        {
            var gameObject = new GameObject();
            var grid = gameObject.AddComponent<NodeGrid>();
            grid.transform.position = Vector3.zero;

            Node node = grid.GetNodeFromVector2(new Vector2(Node.NODE_DIAMETER * 40, Node.NODE_DIAMETER * 40));

            grid.MakeDimensionsOccupied(node, 3, 3);

            Node nodeTopLeft = grid.GetNodeFromVector2(new Vector2(Node.NODE_DIAMETER * 39, Node.NODE_DIAMETER * 41));
            Node nodeTopMid = grid.GetNodeFromVector2(new Vector2(Node.NODE_DIAMETER * 40, Node.NODE_DIAMETER * 41));
            Node nodeTopRight = grid.GetNodeFromVector2(new Vector2(Node.NODE_DIAMETER * 41, Node.NODE_DIAMETER * 41));

            Node nodeLeft = grid.GetNodeFromVector2(new Vector2(Node.NODE_DIAMETER * 39, Node.NODE_DIAMETER * 40));
            Node nodeRight = grid.GetNodeFromVector2(new Vector2(Node.NODE_DIAMETER * 41, Node.NODE_DIAMETER * 40));

            Node nodeBottomLeft = grid.GetNodeFromVector2(new Vector2(Node.NODE_DIAMETER * 39, Node.NODE_DIAMETER * 39));
            Node nodeBottomMid = grid.GetNodeFromVector2(new Vector2(Node.NODE_DIAMETER * 40, Node.NODE_DIAMETER * 39));
            Node nodeBottomRight = grid.GetNodeFromVector2(new Vector2(Node.NODE_DIAMETER * 41, Node.NODE_DIAMETER * 39));

            Assert.IsTrue(nodeTopLeft.IsOccupied);
            Assert.IsTrue(nodeTopMid.IsOccupied);
            Assert.IsTrue(nodeTopRight.IsOccupied);

            Assert.IsTrue(nodeLeft.IsOccupied);
            Assert.IsTrue(nodeRight.IsOccupied);

            Assert.IsTrue(nodeBottomLeft.IsOccupied);
            Assert.IsTrue(nodeBottomMid.IsOccupied);
            Assert.IsTrue(nodeBottomRight.IsOccupied);

            Assert.IsTrue(node.IsOccupied);
        }
    }
}

using NUnit.Framework;
using UnityEngine;
using FarmSim.Grid;

namespace Tests
{
    public class GridLayoutTests
    {
        [Test]
        public void GridLayoutTestsSimplePasses()
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
    }
}

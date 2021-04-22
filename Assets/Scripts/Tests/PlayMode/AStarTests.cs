using FarmSim.Grid;
using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;

public class AStarTests
{
    private NodeGrid currNodeGrid;
    private PathRequestManager currRequestManager;

    [SetUp]
    public void SetUp()
    {
        currNodeGrid = new GameObject().AddComponent<NodeGrid>();

        currNodeGrid.transform.position = Vector3.zero;
        currNodeGrid.LoadSectionTest();

        currRequestManager = currNodeGrid.gameObject.AddComponent<PathRequestManager>();
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(currNodeGrid.gameObject);
    }

    [UnityTest]
    public IEnumerator AStarPathFindingNoBlocksTest()
    {
        float xStart = Node.NODE_DIAMETER * 5;
        float yStart = Node.NODE_DIAMETER * 5;

        float xEnd = Node.NODE_DIAMETER * 1;
        float yEnd = Node.NODE_DIAMETER * 2;

        // 5 * i i i i S
        // 4 * i * * * *
        // 3 * i * * * *
        // 2 * E * * * *
        // 1 * * * * * *
        // 0 * * * * * *
        // 0 0 1 2 3 4 5

        var start = new Vector2(xStart, yStart);
        var end = new Vector2(xEnd, yEnd);

        Node startNode = currNodeGrid.GetNodeFromVector2(start);
        Node endNode = currNodeGrid.GetNodeFromVector2(end);

        Vector2[] path = null;
        PathRequest request = new PathRequest("test", startNode, endNode, (Vector2[] _path, bool foundPath) =>
        {
            path = _path;
            Assert.IsTrue(foundPath);
        });

        currRequestManager.RequestPath(request);

        yield return new WaitForSeconds(0.1f);

        Assert.NotNull(path);

        Assert.AreEqual(7, currNodeGrid.GetManhattanDistance(startNode, endNode));
        Assert.AreEqual(7, path.Length);
    }

    [UnityTest]
    public IEnumerator AStarPathFindingWithBlockageTest()
    {
        float xStart = Node.NODE_DIAMETER * 5;
        float yStart = Node.NODE_DIAMETER * 5;

        float xEnd = Node.NODE_DIAMETER * 1;
        float yEnd = Node.NODE_DIAMETER * 2;


        // 5 i i i i i S
        // 4 i - - | * *
        // 3 i i * | * *
        // 2 * E * | * *
        // 1 * * * | * *
        // 0 * * * * * *
        // 0 0 1 2 3 4 5

        var start = new Vector2(xStart, yStart);
        var end = new Vector2(xEnd, yEnd);
        Debug.Log(currNodeGrid);

        Node startNode = currNodeGrid.GetNodeFromVector2(start);
        Node endNode = currNodeGrid.GetNodeFromVector2(end);

        // create the walls
        Node blockage_1_4 = currNodeGrid.GetNodeFromVector2(new Vector2(Node.NODE_DIAMETER * 1, Node.NODE_DIAMETER * 4));
        Node blockage_2_4 = currNodeGrid.GetNodeFromVector2(new Vector2(Node.NODE_DIAMETER * 2, Node.NODE_DIAMETER * 4));
        Node blockage_3_4 = currNodeGrid.GetNodeFromVector2(new Vector2(Node.NODE_DIAMETER * 3, Node.NODE_DIAMETER * 4));
        Node blockage_3_3 = currNodeGrid.GetNodeFromVector2(new Vector2(Node.NODE_DIAMETER * 3, Node.NODE_DIAMETER * 3));
        Node blockage_3_2 = currNodeGrid.GetNodeFromVector2(new Vector2(Node.NODE_DIAMETER * 3, Node.NODE_DIAMETER * 2));
        Node blockage_3_1 = currNodeGrid.GetNodeFromVector2(new Vector2(Node.NODE_DIAMETER * 3, Node.NODE_DIAMETER * 1));

        // make the walls appear as walls to the algorithm
        blockage_1_4.Data.IsWalkable = false;
        blockage_2_4.Data.IsWalkable = false;
        blockage_3_4.Data.IsWalkable = false;
        blockage_3_3.Data.IsWalkable = false;
        blockage_3_2.Data.IsWalkable = false;
        blockage_3_1.Data.IsWalkable = false;

        Vector2[] path = null;
        PathRequest request = new PathRequest("blockage_test", startNode, endNode, (Vector2[] _path, bool foundPath) =>
        {
            path = _path;
            Assert.IsTrue(foundPath);
        });

        currRequestManager.RequestPath(request);

        yield return new WaitForSeconds(0.1f);

        Assert.NotNull(path);

        Assert.AreEqual(7, currNodeGrid.GetManhattanDistance(startNode, endNode));
        Assert.AreEqual(9, path.Length);

        Node prevNode = startNode;

        for (int i = 0; i < path.Length; i++)
        {
            Node node = currNodeGrid.GetNodeFromVector2(path[i]);
            if (i <= 4)
            {
                // the previous node should be one to the right of the current node during steps <= 4
                Assert.IsTrue((prevNode.Data.x == node.Data.x + 1) && (prevNode.Data.y == node.Data.y));
            }
            else if ((i == 5) || (i == 6))
            {
                // the previous node should be one above the current node during steps 5 and 6
                Assert.IsTrue((prevNode.Data.y == node.Data.y + 1) && (prevNode.Data.x == node.Data.x));
            }

            prevNode = node;
        }
    }
}
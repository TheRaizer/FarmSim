using System.Collections;
using System.Collections.Generic;
using FarmSim.Grid;
using FarmSim.Player;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PlayerControllerTests
{
    [UnityTest]
    public IEnumerator PlayerPathFindingTest()
    {
        var playerPrefab = Resources.Load("Prefabs/UnitTests/PlayerUnitTest") as GameObject;
        if (playerPrefab == null)
        {
            Debug.LogError("There is no unit test prefab at path: Prefabs/UnitTests/PlayerUnitTest");
        }
        var playerObj = Object.Instantiate(playerPrefab);
        playerObj.transform.position = Vector3.zero;

        var playerController = playerObj.GetComponent<PlayerController>();

        var grid = new GameObject().AddComponent<NodeGrid>();
        grid.transform.position = Vector3.zero;
        grid.LoadSectionTest();

        yield return new WaitForSeconds(3);
    }
}

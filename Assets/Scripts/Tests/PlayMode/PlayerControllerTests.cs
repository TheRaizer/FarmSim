using System.Collections;
using System.Collections.Generic;
using FarmSim.Grid;
using FarmSim.Player;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class PlayerControllerTests
{
    [UnityTest]
    public IEnumerator PlayerPathFindingTest()
    {
        yield return new WaitForSeconds(3);
    }
}

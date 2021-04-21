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
        SceneManager.LoadScene(1);

        yield return new WaitForSeconds(3);
    }
}

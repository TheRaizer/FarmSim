using System.Collections;
using System.Collections.Generic;
using FarmSim.Grid;
using FarmSim.Player;
using NUnit.Framework;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class PlayerControllerTests
{
    [OneTimeSetUp]
    public void SetUp()
    {
        EditorSceneManager.LoadSceneInPlayMode
            (
            "Assets/Resources/Scenes/Test_Scene.unity",
            new LoadSceneParameters(LoadSceneMode.Single, LocalPhysicsMode.Physics2D)
            );
    }

    [UnityTest]
    public IEnumerator Test()
    {
        yield return new WaitForSeconds(1f);
    }

    [UnityTest]
    public IEnumerator PlayerPathFindingTest()
    {
        yield return new WaitForSeconds(1f);
    }
}

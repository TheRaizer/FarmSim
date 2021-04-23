using System.Collections;
using UnityEngine.Assertions;

public class TestUtilities
{
    public static IEnumerator AssertSceneLoaded(string scenePath)
    {
        var waitForScene = new WaitForSceneLoaded(scenePath);
        yield return waitForScene;
        Assert.IsFalse(waitForScene.TimedOut, "Scene at path (" + scenePath + ") was never loaded");
    }
}

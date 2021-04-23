using UnityEngine;
using UnityEngine.SceneManagement;

public class WaitForSceneLoaded : CustomYieldInstruction
{
    readonly string scenePath;
    readonly float timeout;
    readonly float startTime;
    bool timedOut;

    public bool TimedOut => timedOut;

    public override bool keepWaiting
    {
        get
        {
            var scene = SceneManager.GetSceneByPath(scenePath);
            var sceneLoaded = scene.IsValid() && scene.isLoaded;

            if (Time.realtimeSinceStartup - startTime >= timeout)
            {
                timedOut = true;
            }

            return !sceneLoaded && !timedOut;
        }
    }

    public WaitForSceneLoaded(string newScenePath, float newTimeout = 10)
    {
        scenePath = newScenePath;
        timeout = newTimeout;
        startTime = Time.realtimeSinceStartup;
    }
}

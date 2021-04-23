using UnityEngine;
using UnityEngine.SceneManagement;

public class WaitForSceneLoaded : CustomYieldInstruction
{
    private readonly string scenePath;
    private readonly float timeout;
    private readonly float startTime;
    private bool timedOut;

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

    public WaitForSceneLoaded(string _scenePath, float _timeout = 10)
    {
        scenePath = _scenePath;
        timeout = _timeout;
        startTime = Time.realtimeSinceStartup;
    }
}

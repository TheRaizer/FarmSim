using System.Collections.Generic;
using System;
using UnityEngine;

public class OnPress : MonoBehaviour
{
    public readonly List<Action> actions = new List<Action>();

    /// <summary>
    ///     Event attached in the Unity editor.
    /// </summary>
    private void OnInteract()
    {
        foreach(Action a in actions)
        {
            a?.Invoke();
        }
    }
}

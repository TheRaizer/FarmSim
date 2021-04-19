using FarmSim.TimeBased;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sprinkler : MonoBehaviour, ITimeBased
{
    [Header("Sprinkling distance")]
    [SerializeField] private int xDist = 1;
    [SerializeField] private int yDist = 1;

    [Header("Animation Settings")]
    [SerializeField] private float animationInterval = 5f;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void OnDayPass()
    {
        // water dirt
    }
}

using FarmSim.TimeBased;
using FarmSim.Grid;
using FarmSim.Items;
using FarmSim.Enums;
using FarmSim.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using FarmSim.Planteables;

public class Sprinkler : MonoBehaviour, ITimeBased, IInteractable
{
    [SerializeField] private ItemType sprinklerItem;

    [Header("Sprinkling distance")]
    [SerializeField] private int xDist = 1;
    [SerializeField] private int yDist = 1;

    [Header("Animation Settings")]
    [SerializeField] private float animationInterval = 15f;
    [SerializeField] private string animationName = "Sprinkle";

    private Animator animator;

    private readonly WaitForSeconds initialWait = new WaitForSeconds(1.5f);
    private WaitForSeconds animationWait;

    /// <summary>
    ///     The previous <see cref="IInteractable"/> that was held in the <see cref="Node"/> this sprinkler lies on.
    ///     
    ///     <para>
    ///         The <see cref="Node"/>'s <see cref="IInteractable"/> is reassigned  to this when the sprinkler is removed.
    ///     </para>
    /// </summary>
    private IInteractable prevInteractable;

    public int X { get; set; }
    public int Y { get; set; }

    private readonly string guid = Guid.NewGuid().ToString();

    private void Awake()
    {
        animator = GetComponent<Animator>();
        animationWait = new WaitForSeconds(animationInterval);

        InitializeNodeInfo();
        AddAsWaterSource();
    }

    private void InitializeNodeInfo()
    {
        Node node = NodeGrid.Instance.GetNodeFromVector2(gameObject.transform.position);
        X = node.Data.x;
        Y = node.Data.y;
        prevInteractable = node.Interactable;
        node.Interactable = this;
    }

    private IEnumerator SprinkleCo()
    {
        yield return initialWait;

        StopCoroutine(AnimationCo());
        StartCoroutine(AnimationCo());
    }

    private IEnumerator AnimationCo()
    {
        animator.SetTrigger(animationName);

        yield return animationWait;

        StartCoroutine(AnimationCo());
    }

    private void WaterDimensions()
    {
        Node middleNode = NodeGrid.Instance.GetNodeFromXY(X, Y);

        if (middleNode == null)
            Debug.LogError("Middle node of a sprinkler was null");

        List<Node> nodesToWater = NodeGrid.Instance.GetNodesFromDimensions(middleNode, xDist, yDist);

        foreach (Node n in nodesToWater)
        {
            n.Interactable.OnInteract(ToolTypes.WateringCan);
        }
    }

    private void AddAsWaterSource()
    {
        Node middleNode = NodeGrid.Instance.GetNodeFromXY(X, Y);

        if (middleNode == null)
            Debug.LogError("Middle node of a sprinkler was null");

        List<Node> nodesToWater = NodeGrid.Instance.GetNodesFromDimensions(middleNode, xDist, yDist);

        foreach (Node n in nodesToWater)
        {
            // if the interactable 
            if (n.Interactable)
            {
                referenceList.Guids.Add(guid);
            }
        }
    }

    public void OnTimePass(int daysPassed = 1)
    {
        StartCoroutine(SprinkleCo());
    }

    public void OnInteract(ToolTypes toolType, GameObject gameObject = null, Action onSuccessful = null)
    {
        if(toolType == ToolTypes.Hoe)
        {
            RemoveSprinkler();
        }
    }

    private void RemoveSprinkler()
    {
        // drop the world item
        var worldItem = Instantiate(sprinklerItem.WorldItemPrefab);
        worldItem.transform.position = gameObject.transform.position;

        Node middleNode = NodeGrid.Instance.GetNodeFromXY(X, Y);

        // reassign to the previous interactable
        middleNode.Interactable = prevInteractable;

        Destroy(gameObject);
    }
}

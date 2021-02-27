﻿using UnityEngine;
using FarmSim.Grid;

/// <class name="MoveObject">
///     <summary>
///         Manages the movement of a single IPlaceable object at a time.
///     </summary>
/// </class>
public class MoveObject : MonoBehaviour
{
    private NodeGrid grid;
    public Placeable AttachedObject { private get; set; }
    private Node currentNode = null;

    private void Awake()
    {
        grid = FindObjectOfType<NodeGrid>();
    }

    private void Update()
    {
        if(AttachedObject != null)
        {
            MovePlaceableToNode();
        }
    }

    /// <summary>
    ///     Moves some IPlaceable to its closest node.
    /// </summary>
    private void MovePlaceableToNode()
    {
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Node node = grid.GetNodeFromVector2(worldPosition);

        if (node != null && (Vector2)AttachedObject.transform.position != node.Position)
        {
            currentNode = node;

            AttachedObject.Node = currentNode;
            AttachedObject.ChangePosition(currentNode.Position);
        }
    }
}

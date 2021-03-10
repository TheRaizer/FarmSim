using FarmSim.Enums;
using System;
using UnityEngine;

namespace FarmSim.Grid
{
    public interface IInteractable
    {
        int X { get; set; }
        int Y { get; set; }
        TileTypes TileType { get; }
        void OnInteract(ToolTypes toolType, GameObject gameObject = null, Action onSuccessful = null);
    }
}
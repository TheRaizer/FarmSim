using FarmSim.Enums;
using UnityEngine;

namespace FarmSim.Grid
{
    public interface IInteractable
    {
        TileTypes TileType { get; }
        void OnInteract(ToolTypes toolType, GameObject gameObject);
    }
}
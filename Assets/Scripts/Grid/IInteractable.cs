using FarmSim.Enums;
using System;
using UnityEngine;

namespace FarmSim.Grid
{
    public interface IInteractable
    {
        /// <summary>
        ///     The X position of the node in the grid.
        /// </summary>
        int X { get; set; }

        /// <summary>
        ///     The X position of the node in the grid.
        /// </summary>
        int Y { get; set; }

        /// <summary>
        ///     Interacts with the IInteractable.
        /// </summary>
        /// <param name="toolType">The type of tool that is in use.</param>
        /// <param name="gameObject">(optional) The gameObject that may be used when interacting.</param>
        /// <param name="onSuccessful">(optional) A void delegate to run if the interation was succesful.</param>
        void OnInteract(ToolTypes toolType, GameObject gameObject = null, Action onSuccessful = null);
    }
}
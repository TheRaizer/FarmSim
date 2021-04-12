using UnityEngine.UI;
using UnityEngine;

namespace FarmSim.Slots
{
    /// <summary>
    ///     A <see cref="GameObject"/> whose position can be swapped with another <see cref="ISwappable"/> in UI Slots managed by <see cref="Slots.SlotsHandler"/>'s.
    /// </summary>
    public interface ISwappable
    {
        /// <summary>
        ///     The <see cref="Image"/> component whose <see cref="GameObject"/>'s position will be moved.
        /// </summary>
        Image Icon { get; }

        /// <summary>
        ///     Points to a given <see cref="Image"/> in <see cref="SlotsHandler.slots"/> that parents this <see cref="GameObject"/>.
        /// </summary>
        int SlotIndex { get; set; }

        SlotsHandler SlotsHandler { get; set; }

        /// <summary>
        ///     A function that gives the option to avoid the swap if it returns true.
        /// </summary>
        /// <param name="other">The other Icon's position manager.</param>
        /// <returns>true if the swap was avoidable, false otherwise.</returns>
        bool AvoidSwap(ISwappable other);
        void OnDestroy();
    }
}

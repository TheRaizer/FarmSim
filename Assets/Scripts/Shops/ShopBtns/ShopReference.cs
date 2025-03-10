using UnityEngine;

namespace FarmSim.Shops
{
    /// <class name="ShopReference">
    ///     <summary>
    ///         Abstract class for any component whose actions are specific to the shop that is currently opened.
    ///     </summary>
    /// </class>
    public abstract class ShopReference : MonoBehaviour
    {
        /// <summary>
        ///     Set when the specific shop is opened.
        /// </summary>
        public Shop Shop { protected get; set; }
    }
}
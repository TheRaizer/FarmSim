
namespace FarmSim.Serialization
{
    /// <class name="ItemData">
    ///     <summary>
    ///         Serializable data that an item contains.
    ///     </summary>
    /// </class>
    [System.Serializable]
    public class ItemData
    {
        public int amt;
        public int slotIndex;
        /// <summary>
        ///     The 'name' attribute of the ItemType ScriptableObject this Item would reference. (Corrosponds to an SO file name in the Resources folder)
        /// </summary>
        public string itemTypeName;

        public ItemData(int _amt, string _itemTypeName, int _slotIndex)
        {
            amt = _amt;
            itemTypeName = _itemTypeName;
            slotIndex = _slotIndex;
        }
    }
}

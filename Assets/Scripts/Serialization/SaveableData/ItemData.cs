
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

        /// <summary>
        ///     The 'name' attribute of the ItemType ScriptableObject this Item would reference. (Corrosponds to an SO file name in the Resources folder)
        /// </summary>
        public string itemTypeName;

        public ItemData(int _amt, string _itemTypeName)
        {
            amt = _amt;
            itemTypeName = _itemTypeName;
        }
    }
}

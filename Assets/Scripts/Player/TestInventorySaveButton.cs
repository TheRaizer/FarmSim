using FarmSim.Serialization;
using UnityEngine;

namespace FarmSim.Player 
{
    public class TestInventorySaveButton : MonoBehaviour
    {
        private PlayerInventory inventory;

        private void Awake()
        {
            inventory = FindObjectOfType<PlayerInventory>();
        }

        private void OnMouseDown()
        {
            SaveInventory();
        }

        /// <summary>
        ///     Saves the players inventory then saves the SaveData class to a file.
        /// </summary>
        private void SaveInventory()
        {
            inventory.SaveInventory();
            if (SerializationManager.Save(SaveData.current))
            {
                Debug.Log("Save was succesful");
            }
        }
    }
}

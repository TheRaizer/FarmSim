using FarmSim.Enums;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private List<PlantItem> plantItems;

    private readonly Dictionary<PlantTypes, PlantItem> plantDict = new Dictionary<PlantTypes, PlantItem>();

    private void Awake()
    {
        plantItems.ForEach(plant => plantDict.Add(plant.PlantType, plant));
    }

    public void AddToInventory(PlantTypes type, int amt)
    {
        plantDict[type].AddToAmt(amt);
    }
}

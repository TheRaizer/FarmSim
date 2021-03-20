using System.Collections;
using FarmSim.Planteables;
using FarmSim.Player;
using FarmSim.Serialization;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class PlanteableTest
    {
        [Test]
        public void GrowthTest()
        {
            var prefab = Resources.Load("Prefabs/UnitTests/PotatoUnitTest") as GameObject;
            var planteableObj = Object.Instantiate(prefab);

            Planteable plant = planteableObj.GetComponent<Planteable>();
            
            // we can't harvest until grown
            Assert.IsFalse(plant.CanHarvest);

            // grow 7 days
            for(int i = 0; i < 7; i++)
            {
                plant.Grow();
            }

            // we should be able to harvest now
            Assert.IsTrue(plant.CanHarvest);
        }

        [Test]
        public void PlantSaveTest()
        {
            var prefab = Resources.Load("Prefabs/UnitTests/PotatoUnitTest") as GameObject;
            var planteableObj = Object.Instantiate(prefab);
            var planteableObj_2 = Object.Instantiate(prefab);

            Planteable plant = planteableObj.GetComponent<Planteable>();
            Planteable plant_2 = planteableObj_2.GetComponent<Planteable>();
            Assert.AreEqual(SaveData.Current.plantDatas.Count, 0);

            plant.Save();

            Assert.AreEqual(SaveData.Current.plantDatas.Count, 1);

            plant.Save();

            // because SaveData contains the plant already, saving it should not add it again.
            Assert.AreEqual(SaveData.Current.plantDatas.Count, 1);

            plant_2.Save();

            Assert.AreEqual(SaveData.Current.plantDatas.Count, 2);

            plant.OnHarvest();

            // after harvesting the plant should no longer be saved.
            Assert.AreEqual(SaveData.Current.plantDatas.Count, 1);

            plant_2.OnHarvest();

            Assert.AreEqual(SaveData.Current.plantDatas.Count, 0);
        }

        [UnityTest]
        public IEnumerator HarvestTest()
        {
            var prefab = Resources.Load("Prefabs/UnitTests/PotatoUnitTest") as GameObject;
            var planteableObj = Object.Instantiate(prefab);

            GameObject obj = new GameObject();
            var inventory = obj.AddComponent<PlayerInventory>();

            Planteable plant = planteableObj.GetComponent<Planteable>();
            ItemType itemType = Resources.Load("SO/Potato") as ItemType;

            // Harvest the plant
            plant.OnHarvest();

            // make sure that the players inventory has obtained the harvested plant
            Assert.NotNull(inventory.GetItem(itemType));

            yield return null;

            // plant object should be destroyed
            Assert.IsTrue(plant == null);
        }
    }
}

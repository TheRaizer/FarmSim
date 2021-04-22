using FarmSim.Items;
using FarmSim.Planteables;
using FarmSim.Serialization;
using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class PlanteableTest
    {
        private GameObject planteableObj;
        private Planteable plant;

        private readonly GameObject prefab = Resources.Load("Prefabs/UnitTests/PotatoUnitTest") as GameObject;

        [SetUp]
        public void SetUp()
        {
            prefab.transform.position = Vector3.zero;
            if (prefab == null)
            {
                Debug.LogError("There is no unit test prefab at path: Prefabs/UnitTests/PotatoUnitTest");
            }
            planteableObj = Object.Instantiate(prefab);

            plant = planteableObj.GetComponent<Planteable>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(planteableObj);
        }

        [Test]
        public void GrowthTest()
        {
            // we can't harvest until grown
            Assert.IsFalse(plant.CanHarvest);

            // grow 7 days
            for (int i = 0; i < 7; i++)
            {
                plant.Grow();
            }

            // we should be able to harvest now
            Assert.IsTrue(plant.CanHarvest);
        }

        [Test]
        public void CatchUpDaysGrowthTest()
        {
            // we can't harvest until grown
            Assert.IsFalse(plant.CanHarvest);

            plant.Grow(2);

            Assert.IsFalse(plant.CanHarvest);

            plant.Grow(7);

            // we should be able to harvest now
            Assert.IsTrue(plant.CanHarvest);
        }

        [Test]
        public void PlantSaveTest()
        {
            var planteableObj_2 = Object.Instantiate(planteableObj);
            Planteable plant_2 = planteableObj_2.GetComponent<Planteable>();

            Assert.AreEqual(SectionData.Current.plantDatas.Count, 0);

            plant.Save();

            Assert.AreEqual(SectionData.Current.plantDatas.Count, 1);

            plant.Save();

            // because SaveData contains the plant already, saving it should not add it again.
            Assert.AreEqual(SectionData.Current.plantDatas.Count, 1);

            plant_2.Save();

            Assert.AreEqual(SectionData.Current.plantDatas.Count, 2);

            plant.OnHarvest();

            // after harvesting the plant should no longer be saved.
            Assert.AreEqual(SectionData.Current.plantDatas.Count, 1);

            plant_2.OnHarvest();

            Assert.AreEqual(SectionData.Current.plantDatas.Count, 0);
        }

        [UnityTest]
        public IEnumerator HarvestTest()
        {

            var player = new GameObject();

            // setup player trigger for picking up world items
            var collider = player.AddComponent<BoxCollider2D>();
            collider.isTrigger = true;
            collider.size = Vector2.one * 10;

            player.transform.position = Vector3.zero;

            // setup inventory
            GameObject obj = new GameObject();
            var inventory = obj.AddComponent<Inventory>();

            player.tag = "Player";

            Planteable plant = planteableObj.GetComponent<Planteable>();
            ItemType itemType = Resources.Load("SO/Potato") as ItemType;

            if (itemType == null)
            {
                Debug.LogError("There is no ItemType scriptable object at path: SO/Potato");
            }

            // Harvest the plant
            plant.OnHarvest();

            WorldItem[] worldItem = Object.FindObjectsOfType<WorldItem>();

            Assert.IsTrue(worldItem.Length >= 2 && worldItem.Length <= 5);

            yield return new WaitForSeconds(1f);

            // make sure that the players inventory has obtained the harvested plant
            Assert.IsTrue(inventory.Contains(itemType));

            yield return null;

            // plant object should be destroyed
            Assert.IsTrue(plant == null);
        }
    }
}

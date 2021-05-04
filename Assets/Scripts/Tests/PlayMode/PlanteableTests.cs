using FarmSim.Enums;
using FarmSim.Grid;
using FarmSim.Items;
using FarmSim.Loading;
using FarmSim.Planteables;
using FarmSim.Serialization;
using NUnit.Framework;
using System.Collections;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
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
            if (prefab == null)
            {
                Debug.LogError("There is no unit test prefab at path: Prefabs/UnitTests/PotatoUnitTest");
            }
            prefab.transform.position = Vector3.zero;
            planteableObj = Object.Instantiate(prefab);

            plant = planteableObj.GetComponent<Planteable>();
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

            Assert.AreEqual(SectionData.Current.PlantDatas.Count, 0);

            plant.Save();

            Assert.AreEqual(SectionData.Current.PlantDatas.Count, 1);

            plant.Save();

            // because SaveData contains the plant already, saving it should not add it again.
            Assert.AreEqual(SectionData.Current.PlantDatas.Count, 1);

            plant_2.Save();

            Assert.AreEqual(SectionData.Current.PlantDatas.Count, 2);

            plant.OnHarvest(ToolTypes.Sickle);

            // after harvesting the plant should no longer be saved.
            Assert.AreEqual(SectionData.Current.PlantDatas.Count, 1);

            plant_2.OnHarvest(ToolTypes.Sickle);

            Assert.AreEqual(SectionData.Current.PlantDatas.Count, 0);
        }

        [UnityTest]
        public IEnumerator HarvestTest()
        {
            // load the test scene
            EditorSceneManager.LoadSceneInPlayMode
            (
                "Assets/Resources/Scenes/Test_Scene.unity",
                new LoadSceneParameters(LoadSceneMode.Single, LocalPhysicsMode.Physics2D)
            );

            yield return TestUtilities.AssertSceneLoaded("Assets/Resources/Scenes/Test_Scene.unity");

            Inventory inventory = Object.FindObjectOfType<Inventory>();
            NodeGrid grid = Object.FindObjectOfType<NodeGrid>();
            LoadingOrder loadingOrder = Object.FindObjectOfType<LoadingOrder>();
            ItemType itemType = Resources.Load("SO/Potato") as ItemType;

            // wait till grid is loaded
            while (!grid.LoadedSection)
                yield return null;

            // wait till all data is loaded
            while (!loadingOrder.LoadedAll)
                yield return null;

            if (itemType == null)
            {
                Debug.LogError("There is no ItemType scriptable object at path: SO/Potato");
            }

            // get the node we are going to plant on
            Node nodeToPlant = grid.GetNodeFromXY(0, 0);

            // plant the prefab
            bool succesfulPlanting = false;
            nodeToPlant.Interactable.OnInteract(ToolTypes.Hoe);
            nodeToPlant.Interactable.OnInteract(ToolTypes.Other, prefab, () => succesfulPlanting = true);
            Assert.IsTrue(succesfulPlanting);

            // get the single plant we made in this scene
            Planteable plant = Object.FindObjectOfType<Planteable>();

            // Harvest the plant
            plant.OnHarvest(ToolTypes.Sickle);

            // check if the items dropped
            WorldItem[] worldItem = Object.FindObjectsOfType<WorldItem>();
            Assert.IsTrue(worldItem.Length >= 2 && worldItem.Length <= 5);

            yield return null;

            // plant object should be destroyed
            Assert.IsTrue(plant == null);
        }
    }
}

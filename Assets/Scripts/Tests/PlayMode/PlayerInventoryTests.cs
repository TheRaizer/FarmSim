using FarmSim.Items;
using FarmSim.Serialization;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

namespace Tests
{
    public class PlayerInventoryTests
    {

        private const int POTATO_AMT = 5;
        private const int TOMATO_AMT = 10;
        private const int HOUSE_AMT = 11;

        private Inventory inventory;

        private ItemType tomatoType;
        private ItemType potatoType;
        private ItemType houseType;

        private Item tomato;
        private Item potato;
        private Item house;

        [SetUp]
        public void SetUp()
        {
            inventory = new GameObject().AddComponent<Inventory>();

            tomatoType = Resources.Load("SO/Tomato") as ItemType;
            potatoType = Resources.Load("SO/Potato") as ItemType;
            houseType = Resources.Load("SO/House") as ItemType;
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(inventory.gameObject);
        }

        private void AddToInventory()
        {
            // add to the inventory
            inventory.AddToInventory(tomatoType, TOMATO_AMT);
            inventory.AddToInventory(potatoType, POTATO_AMT);
            inventory.AddToInventory(houseType, HOUSE_AMT);

            // take the item but not any of the amounts
            tomato = inventory.GetFirstInstance(tomatoType);
            potato = inventory.GetFirstInstance(potatoType);
            house = inventory.GetFirstInstance(houseType);

            // make sure that the items have the correct amount added
            Assert.AreEqual(TOMATO_AMT, tomato.Amt);
            Assert.AreEqual(POTATO_AMT, potato.Amt);
            Assert.AreEqual(HOUSE_AMT, house.Amt);
        }

        [Test]
        public void AddInventoryTest()
        {
            AddToInventory();

            // add another potato
            inventory.AddToInventory(potatoType, POTATO_AMT);

            // confirm that it stacks onto the previous potato
            Assert.AreEqual(POTATO_AMT * 2, potato.Amt);

            // overflow the tomato item
            inventory.AddToInventory(tomatoType, 200);

            // make sure that the tomato we first added had maxed out
            Assert.AreEqual(tomato.itemType.MaxCarryAmt, tomato.Amt);

            // obtain all the tomato items in the inventory
            List<Item> tomatoes = inventory.FindInstances(tomatoType);

            // due to overflow there should be exactly 2 tomato objects in the inventory.
            int instancesOfTomato = tomatoes.Count;

            Assert.AreEqual(3, instancesOfTomato);

            // due to overflow the second tomato object should have amount of 10.
            bool foundTomatoAmt10 = false;

            tomatoes.ForEach(x =>
            {
                if (x.Amt == 10)
                {
                    foundTomatoAmt10 = true;
                }
            });

            Assert.IsTrue(foundTomatoAmt10);
        }

        [Test]
        public void IterationAddInventoryTest()
        {
            // add to the inventory
            inventory.AddToInventory(tomatoType, TOMATO_AMT);

            // take the item but not any of the amounts
            Item tomato = inventory.GetFirstInstance(tomatoType);

            for (int j = 1; j <= 2; j++)
            {

                for (int i = 0; i < 10; i++)
                {
                    inventory.AddToInventory(tomatoType, TOMATO_AMT);
                }

                // make sure that the tomato we first added had maxed out
                Assert.AreEqual(tomato.itemType.MaxCarryAmt, tomato.Amt);

                // obtain all the tomato items in the inventory
                List<Item> tomatoes = inventory.FindInstances(tomatoType);

                // due to overflow there should be exactly 1 + j tomato objects in the inventory.
                int instancesOfTomato = tomatoes.Count;

                Assert.AreEqual(1 + j, instancesOfTomato);
            }
        }

        [Test]
        public void TakeFromInventoryTest()
        {
            AddToInventory();

            // take more of the tomato item then there is available.
            Item item = inventory.TakeFromInventory(tomato.guid, TOMATO_AMT + 1);

            // the item returned should still be the tomato item.
            Assert.IsNull(item);

            // item amount should be untouched
            Assert.AreEqual(TOMATO_AMT, tomato.Amt);

            inventory.TakeFromInventory(tomato.guid, tomato.Amt);

            Assert.AreEqual(0, tomato.Amt);

            // the tomato item should be removed from the inventory
            Assert.AreEqual(inventory.FindInstances(tomatoType).Count, 0);
        }

        [Test]
        public void InventorySaveTest() 
        {
            AddToInventory();

            inventory.Save();

            Assert.AreEqual(3, PlayerData.Current.itemDatas.Count);
        }
    }
}

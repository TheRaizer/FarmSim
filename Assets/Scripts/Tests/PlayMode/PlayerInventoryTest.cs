using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using FarmSim.Player;

namespace Tests
{
    public class PlayerInventoryTest
    {
        [Test]
        public void AddInventoryTest()
        {
            const int POTATO_AMT = 5;
            const int TOMATO_AMT = 10;
            const int HOUSE_AMT = 11;

            GameObject gameObject = new GameObject();
            PlayerInventoryList inventory = gameObject.AddComponent<PlayerInventoryList>();

            ItemType tomatoType = Resources.Load("SO/Tomato") as ItemType;
            ItemType potatoType = Resources.Load("SO/Potato") as ItemType;
            ItemType houseType = Resources.Load("SO/House") as ItemType;

            inventory.AddToInventory(potatoType, POTATO_AMT);
            inventory.AddToInventory(houseType, HOUSE_AMT);
            inventory.AddToInventory(tomatoType, TOMATO_AMT);

            Item tomato = inventory.TakeFromInventory(tomatoType, 0);
            Item potato = inventory.TakeFromInventory(potatoType, 0);
            Item house = inventory.TakeFromInventory(houseType, 0);

            Assert.AreEqual(TOMATO_AMT, tomato.Amt);
            Assert.AreEqual(POTATO_AMT, potato.Amt);
            Assert.AreEqual(HOUSE_AMT, house.Amt);

            inventory.AddToInventory(potatoType, POTATO_AMT);

            Assert.AreEqual(POTATO_AMT * 2, potato.Amt);

            inventory.AddToInventory(tomatoType, 100);

            Assert.AreEqual(100, tomato.Amt);

            inventory.inventory.ForEach(x => Debug.Log($"Item: {x.itemType.ItemName} || Amt: {x.Amt}"));
        }
    }
}

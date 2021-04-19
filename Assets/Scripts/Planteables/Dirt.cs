using FarmSim.Attributes;
using FarmSim.Enums;
using FarmSim.Grid;
using FarmSim.Loading;
using FarmSim.Serialization;
using FarmSim.TimeBased;
using FarmSim.Utility;
using System;
using UnityEngine;

namespace FarmSim.Planteables
{
    /// <class name="Dirt">
    ///     <summary>
    ///         Manages the state of the dirt and the plant it contains if there is any.
    ///     </summary>
    /// </class>
    [Savable(false)]
    public class Dirt : MonoBehaviour, IOccurPostLoad, ITimeBased, IInteractable, ISavable, ILoadable
    {
        [SerializeField] private Sprite dryDirt = null;
        [SerializeField] private Sprite hoedDirt = null;
        [SerializeField] private Sprite wetHoedDirt = null;

        public Planteable Plant { private get; set; } = null;

        public DirtData Data { private get; set; }

        public int X { get; set; }
        public int Y { get; set; }

        private int daysTillRevert = 0;

        private SpriteRenderer spriteRenderer = null;
        private ObjectPooler objectPooler = null;

        private const int MAX_HOED_DAYS = 8;
        private const int MIN_HOED_DAYS = 3;


        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            objectPooler = FindObjectOfType<ObjectPooler>();
        }

        public void OnDayPass()
        {
            if (Plant == null)
            {
                daysTillRevert--;
                CheckIfDried();
            }
            else
            {
                if (Data.Watered)
                {
                    Plant.Grow();
                }
            }
            Data.Watered = false;
            CheckSpriteType();
        }

        /// <summary>
        ///     Hoes the dirt changing it from dried dirt to hoed dirt.
        ///     Reinitializes the number of days before it returns back to dried dirt.
        /// </summary>
        private void Hoe()
        {
            if (!Data.Hoed)
            {
                objectPooler.SpawnGameObject("HoedDirtParticles", transform.position, Quaternion.identity);
                Data.Hoed = true;
                daysTillRevert = UnityEngine.Random.Range(MIN_HOED_DAYS, MAX_HOED_DAYS);
                spriteRenderer.sprite = hoedDirt;
            }
        }

        /// <summary>
        ///     Waters the dirt and if the dirt is hoed it will make the sprite change.
        /// </summary>
        private void Water()
        {
            Data.Watered = true;

            CheckSpriteType();
        }

        private void Harvest()
        {
            if (Plant != null && Plant.CanHarvest)
            {
                Plant.OnHarvest();
                Node node = NodeGrid.Instance.GetNodeFromXY(X, Y);
                node.Data.IsOccupied = false;
            }
        }

        /// <summary>
        ///     Checks to see if it is time to revert back to dried dirt.
        /// </summary>
        private void CheckIfDried()
        {
            if (daysTillRevert <= 0)
            {
                Data.Hoed = false;
                Data.Watered = false;
                CheckSpriteType();
            }
        }

        private void CheckSpriteType()
        {
            if (Data.Hoed)
            {
                if (Data.Watered)
                {
                    spriteRenderer.sprite = wetHoedDirt;
                }
                else
                {
                    spriteRenderer.sprite = hoedDirt;
                }
            }
            else
            {
                spriteRenderer.sprite = dryDirt;
            }
        }

        public void OnInteract(ToolTypes toolType, GameObject gameObject, Action onSuccessful)
        {
            switch (toolType)
            {
                case ToolTypes.Hoe:
                    Hoe();
                    onSuccessful?.Invoke();
                    break;
                case ToolTypes.WateringCan:
                    Water();
                    onSuccessful?.Invoke();
                    break;
                case ToolTypes.Sickle:
                    if (Plant == null || Plant.ToolToHarvestWith != ToolTypes.Sickle)
                        return;
                    Harvest();
                    onSuccessful?.Invoke();
                    break;
                case ToolTypes.Axe:
                    if (Plant == null || Plant.ToolToHarvestWith != ToolTypes.Axe)
                        return;
                    Harvest();
                    onSuccessful?.Invoke();
                    break;
                case ToolTypes.Other:
                    // check if the given gameObject contains a Planteable component
                    if (Data.Hoed && gameObject != null && gameObject.TryGetComponent<Planteable>(out _))
                    {
                        // generate the Planteable GameObject
                        var obj = Instantiate(gameObject);
                        obj.transform.position = transform.position;

                        // initialize the Dirts Plant field and give it the same Id as this Dirt instance.
                        Plant = obj.GetComponent<Planteable>();
                        Plant.SetDataId(Data.Id);

                        onSuccessful?.Invoke();
                    }
                    break;
                default:
                    /*Debug.Log($"Do nothing with tooltype {toolType}");*/
                    break;
            }
        }

        public void Save()
        {
            if (!SectionData.Current.dirtDatas.Contains(Data))
            {
                SectionData.Current.dirtDatas.Add(Data);
            }
        }

        public void Load()
        {
            bool isEmpty = SectionData.Current.dirtDatas == null || SectionData.Current.dirtDatas.Count <= 0;
            if (isEmpty)
            {
                // if there is no dirt data that was loaded then create a new one.
                Data = new DirtData(UniqueIdGenerator.IdFromDate(), X, Y, false, false, daysTillRevert);
            }
            else
            {
                LoadExistingDirt();
            }
        }

        private void LoadExistingDirt()
        {
            // find the dirts data that matches its x and y.
            Data = SectionData.Current.dirtDatas.Find(dirt => X == dirt.x && Y == dirt.y);

            PlanteableData plantData = SectionData.Current.plantDatas.Find(plant => plant.Id == Data.Id);

            // if there is a matching plant data
            if (plantData != null)
            {
                // we must create a plant game object
                var gameObject = Resources.Load("Prefabs/Planteables/" + plantData.PrefabName) as GameObject;

                if (gameObject == null)
                {
                    Debug.LogError("There is no planteable prefab at path: " + "Prefabs/Planteables/" + plantData.PrefabName);
                }

                var objInstance = Instantiate(gameObject);
                objInstance.transform.position = transform.position;

                // assign the plant data
                Plant = objInstance.GetComponent<Planteable>();
                Plant.Data = plantData;
            }
        }

        public void PostLoad()
        {
            CheckSpriteType();
        }
    }
}

using FarmSim.Attributes;
using FarmSim.Enums;
using FarmSim.Grid;
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
    public class Dirt : TimeCatchUp, IInteractable, ISavable, ILoadable, IWaterSourceRefsGUIDs
    {
        [SerializeField] private Sprite dryDirt = null;
        [SerializeField] private Sprite hoedDirt = null;
        [SerializeField] private Sprite wetHoedDirt = null;

        private const string PLANTEABLE_PREFAB_FOLDER = "Prefabs/Planteables/";

        private Planteable plant = null;
        private NodeGrid nodeGrid;

        private DirtData data;

        public int X { get; set; }
        public int Y { get; set; }

        private SpriteRenderer spriteRenderer = null;
        private ObjectPooler objectPooler = null;

        private const int MAX_HOED_DAYS = 8;
        private const int MIN_HOED_DAYS = 3;


        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            objectPooler = FindObjectOfType<ObjectPooler>();
            nodeGrid = FindObjectOfType<NodeGrid>();
        }

        public override void OnTimePass(int daysPassed = 1)
        {
            if (plant == null)
            {
                data.DaysTillRevert -= daysPassed;
                CheckIfDried();
            }
            else
            {
                if (data.WaterSrcGuids.Count > 0)
                {
                    Debug.Log("Grow multiple");
                    // if theres a water source we can make multiple days pass
                    plant.Grow(daysPassed);
                }
                else if (data.Watered)
                {
                    // if its just watered only grow it once
                    plant.Grow();
                }
            }
            data.Watered = false;
            CheckSpriteType();
        }

        private void SetDaysTillRevert() => data.DaysTillRevert = UnityEngine.Random.Range(MIN_HOED_DAYS, MAX_HOED_DAYS);

        /// <summary>
        ///     Hoes the dirt changing it from dried dirt to hoed dirt.
        ///     Reinitializes the number of days before it returns back to dried dirt.
        /// </summary>
        private void Hoe()
        {
            if (!data.Hoed)
            {
                objectPooler.SpawnGameObject("HoedDirtParticles", transform.position, Quaternion.identity);
                data.Hoed = true;
                SetDaysTillRevert();
                spriteRenderer.sprite = hoedDirt;
            }
        }

        /// <summary>
        ///     Waters the dirt and if the dirt is hoed it will make the sprite change.
        /// </summary>
        private void Water()
        {
            data.Watered = true;
            SetDaysTillRevert();
            CheckSpriteType();
        }

        private void Harvest()
        {
            if (plant != null && plant.CanHarvest)
            {
                plant.OnHarvest();
                plant = null;
                Node node = nodeGrid.GetNodeFromXY(X, Y);
                node.Data.IsOccupied = false;
            }
        }

        /// <summary>
        ///     Checks to see if it is time to revert back to dried dirt.
        /// </summary>
        private void CheckIfDried()
        {
            if (data.DaysTillRevert <= 0)
            {
                data.Hoed = false;
                data.Watered = false;
                CheckSpriteType();
            }
        }

        private void CheckSpriteType()
        {
            if (data.Hoed)
            {
                if (data.Watered)
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
                    if (plant == null || plant.ToolToHarvestWith != ToolTypes.Sickle)
                        return;
                    Harvest();
                    onSuccessful?.Invoke();
                    break;
                case ToolTypes.Axe:
                    if (plant == null || plant.ToolToHarvestWith != ToolTypes.Axe)
                        return;
                    Harvest();
                    onSuccessful?.Invoke();
                    break;
                case ToolTypes.Other:
                    // check if the given gameObject contains a Planteable component
                    if (data.Hoed && gameObject != null && gameObject.TryGetComponent<Planteable>(out _))
                    {
                        // generate the Planteable GameObject
                        var obj = Instantiate(gameObject);
                        obj.transform.position = transform.position;

                        // initialize the Dirts Plant field and give it the same Id as this Dirt instance.
                        plant = obj.GetComponent<Planteable>();
                        plant.SetDataId(data.Id);
                    }
                    onSuccessful?.Invoke();
                    break;
                default:
                    /*Debug.Log($"Do nothing with tooltype {toolType}");*/
                    break;
            }
        }

        public void Save()
        {
            if (!SectionData.Current.DirtDatas.Contains(data))
            {
                SectionData.Current.DirtDatas.Add(data);
            }
        }

        public void Load()
        {
            bool noDirt = SectionData.Current.DirtDatas == null || SectionData.Current.DirtDatas.Count <= 0;
            if (noDirt)
            {
                // if there is no dirt data that was loaded then create a new one.
                data = new DirtData(UniqueIdGenerator.IdFromDate(), X, Y, false, false, 0);
            }
            else
            {
                LoadExistingDirt();
            }
        }

        private void LoadExistingDirt()
        {
            // find the dirts data that matches its x and y.
            data = SectionData.Current.DirtDatas.Find(dirt => X == dirt.x && Y == dirt.y);

            PlanteableData plantData = SectionData.Current.PlantDatas.Find(plant => plant.Id == data.Id);

            // if there is a matching plant data
            if (plantData != null)
            {
                // we must create a plant game object
                var prefab = Resources.Load(PLANTEABLE_PREFAB_FOLDER + plantData.PrefabName) as GameObject;

                if (prefab == null)
                {
                    Debug.LogError("There is no planteable prefab at path: " + PLANTEABLE_PREFAB_FOLDER + plantData.PrefabName);
                }

                var gameObject = Instantiate(prefab);
                gameObject.transform.position = transform.position;

                // assign the plant data
                plant = gameObject.GetComponent<Planteable>();
                plant.Data = plantData;
            }
        }

        public override void PostLoad()
        {
            base.PostLoad();
            CheckSpriteType();
        }

        public void AddToWaterSources(string guid)
        {
            data.WaterSrcGuids.Add(guid);
        }

        public void RemoveFromWaterSources(string guid)
        {
            data.WaterSrcGuids.Remove(guid);
        }
    }
}

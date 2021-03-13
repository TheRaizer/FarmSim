using FarmSim.Enums;
using FarmSim.Grid;
using FarmSim.TimeBased;
using FarmSim.Serialization;
using System;
using UnityEngine;
using System.Collections.Generic;
using FarmSim.Utility;

namespace FarmSim.Planteables
{
    /// <class name="Dirt">
    ///     <summary>
    ///         Manages the state of the dirt and the plant it contains if there is any.
    ///     </summary>
    /// </class>
    public class Dirt : MonoBehaviour, ITimeBased, IInteractable, ISavable
    {
        [SerializeField] private Sprite dryDirt = null;
        [SerializeField] private Sprite hoedDirt = null;
        [SerializeField] private Sprite wetHoedDirt = null;

        public Planteable Plant { private get; set; } = null;

        /// <summary>
        ///     Should be set from a ILoadable that loads the nodes.
        /// </summary>
        public DirtData Data { private get; set; }

        public int X { get; set; }
        public int Y { get; set; }

        private int daysTillRevert = 0;

        private SpriteRenderer spriteRenderer = null;
        private NodeGrid grid = null;

        private const int MAX_HOED_DAYS = 8;
        private const int MIN_HOED_DAYS = 3;


        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            grid = FindObjectOfType<NodeGrid>();
        }

        private void Start()
        {
            if (Data == null)
            {
                Data = new DirtData(UniqueIdGenerator.IdFromDate(), X, Y, false, false, daysTillRevert);
            }
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
            /*watered = false;
            CheckSpriteType();*/
        }

        /// <summary>
        ///     Hoes the dirt changing it from dried dirt to hoed dirt.
        ///     Reinitializes the number of days before it returns back to dried dirt.
        /// </summary>
        private void Hoe()
        {
            if (!Data.Hoed)
            {
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

        private void Sickle()
        {
            if (Plant != null && Plant.CanHarvest)
            {
                Plant.OnHarvest();
                Node node = grid.GetNodeFromXY(X, Y);
                node.IsOccupied = false;
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
                    Sickle();
                    onSuccessful?.Invoke();
                    break;
                case ToolTypes.Other:
                    // check if this gameObject contains a planteable
                    if(gameObject != null && gameObject.TryGetComponent<Planteable>(out _) && Data.Hoed)
                    {
                        var obj = Instantiate(gameObject);
                        obj.transform.position = transform.position;
                        Plant = obj.GetComponent<Planteable>();

                        Plant.SetDataId(Data.Id);

                        onSuccessful?.Invoke();
                    }
                    break;
                default:
                    Debug.Log($"Do nothing with tooltype {toolType}");
                    break;
            }
        }

        public void Save()
        {
            if(SaveData.Current.dirtDatas == null)
            {
                SaveData.Current.dirtDatas = new List<DirtData>();
            }
            SaveData.Current.dirtDatas.Add(Data);
        }
    }
}

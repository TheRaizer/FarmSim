using FarmSim.Enums;
using FarmSim.Grid;
using FarmSim.TimeBased;
using System;
using UnityEngine;

namespace FarmSim.Planteables
{
    /// <class name="Dirt">
    ///     <summary>
    ///         Manages the state of the dirt and the plant it contains if there is any.
    ///     </summary>
    /// </class>
    public class Dirt : MonoBehaviour, ITimeBased, IInteractable
    {
        [SerializeField] private Sprite dryDirt = null;
        [SerializeField] private Sprite hoedDirt = null;
        [SerializeField] private Sprite wetHoedDirt = null;

        private bool hoed = false;
        private bool watered = false;

        private int daysTillRevert = 0;

        private const int MAX_HOED_DAYS = 8;
        private const int MIN_HOED_DAYS = 3;

        private SpriteRenderer spriteRenderer = null;
        private NodeGrid grid = null;

        public Planteable Plant { private get; set; } = null;

        public TileTypes TileType { get; } = TileTypes.Dirt;
        public int X { get; set; }
        public int Y { get; set; }

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            grid = FindObjectOfType<NodeGrid>();
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
                if (watered)
                {
                    Plant.Grow();
                }
            }
            watered = false;
        }

        /// <summary>
        ///     Hoes the dirt changing it from dried dirt to hoed dirt.
        ///     Reinitializes the number of days before it returns back to dried dirt.
        /// </summary>
        private void Hoe()
        {
            if (!hoed)
            {
                hoed = true;
                daysTillRevert = UnityEngine.Random.Range(MIN_HOED_DAYS, MAX_HOED_DAYS);
                spriteRenderer.sprite = hoedDirt;
            }
        }

        /// <summary>
        ///     Waters the dirt and if the dirt is hoed it will make the sprite change.
        /// </summary>
        private void Water()
        {
            watered = true;

            if (hoed)
            {
                spriteRenderer.sprite = wetHoedDirt;
            }
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
                hoed = false;
                watered = false;
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
                    if(gameObject != null && gameObject.TryGetComponent<Planteable>(out _) && hoed)
                    {
                        var obj = Instantiate(gameObject);
                        obj.transform.position = transform.position;
                        Plant = obj.GetComponent<Planteable>();
                        onSuccessful?.Invoke();
                    }
                    break;
                default:
                    Debug.Log($"Do nothing with tooltype {toolType}");
                    break;
            }
        }
    }
}

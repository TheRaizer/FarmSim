﻿using FarmSim.Enums;
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
        public Planteable Plant { private get; set; } = null;

        public TileTypes TileType { get; } = TileTypes.DIRT;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
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
        public void Water()
        {
            watered = true;

            if (hoed)
            {
                spriteRenderer.sprite = wetHoedDirt;
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
                case ToolTypes.HOE:
                    Hoe();
                    onSuccessful?.Invoke();
                    break;
                case ToolTypes.WATERING_CAN:
                    Water();
                    onSuccessful?.Invoke();
                    break;
                case ToolTypes.OTHER:
                    // check if this gameObject contains a planteable
                    if(gameObject != null && gameObject.TryGetComponent<Planteable>(out _) && hoed)
                    {
                        Debug.Log("Plant");
                        var obj = Instantiate(gameObject);
                        obj.transform.position = transform.position;
                        Plant = obj.GetComponent<Planteable>();
                        onSuccessful?.Invoke();
                    }
                    break;
                default:
                    throw new Exception($"Not valid tooltype {toolType}");

            }
        }
    }
}

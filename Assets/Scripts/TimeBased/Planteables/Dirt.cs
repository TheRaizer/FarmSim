using FarmSim.TimeBased;
using UnityEngine;

namespace FarmSim.Planteables
{
    /// <class name="Dirt">
    ///     <summary>
    ///         Manages the state of the dirt and the plant it contains if there is any.
    ///     </summary>
    /// </class>
    public class Dirt : MonoBehaviour, ITimeBased
    {
        [SerializeField] private Sprite dryDirt = null;
        [SerializeField] private Sprite hoedDirt = null;
        [SerializeField] private Sprite wetHoedDirt = null;

        private bool hoed = false;
        private bool watered = false;

        private int daysTillRevert = 0;

        private const int MAX_HOED_DAYS = 6;
        private const int MIN_HOED_DAYS = 2;

        private SpriteRenderer spriteRenderer = null;
        public Planteable Plant { get; set; } = null;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void OnDayPass()
        {
            if (Plant != null)
            {
                daysTillRevert--;
                CheckIfDried();
            }
            else
            {
                if (watered)
                {
                    // grow plant by one day
                }
            }
        }

        /// <summary>
        ///     Hoes the dirt changing it from dried dirt to hoed dirt.
        ///     Reinitializes the number of days before it returns back to dried dirt.
        /// </summary>
        public void Hoe()
        {
            if (!hoed)
            {
                hoed = true;
                daysTillRevert = Random.Range(MIN_HOED_DAYS, MAX_HOED_DAYS);
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
    }
}

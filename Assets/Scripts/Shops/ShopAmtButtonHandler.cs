using FarmSim.Items;
using UnityEngine;
using UnityEngine.EventSystems;

namespace FarmSim.Shops
{
    /// <class name="ShopAmtButtonHandler">
    ///     <summary>
    ///         Manages the amount of an <see cref="Item"/> the player wishes to sell or buy from a given <see cref="Shop"/>.
    ///     </summary>
    /// </class>
    public class ShopAmtButtonHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private Shop shopToHandle;

        /// <summary>
        ///     Whether this UI object is to increment or decrement the amount when clicked.
        /// </summary>
        [SerializeField] private bool increment;

        // The intervals by which the amt changes when this UI object is clicked and held
        private const float MAX_INTER = 0.3f;
        private const float MIN_INTER = 0.01f;
        private const float INTER_DECR = 0.1f;

        private float CurrentInterval;
        private float timer = 0;
        private bool mouseIsDown;

        private void Update()
        {
            if (mouseIsDown)
            {
                if (timer <= 0)
                {
                    ChangeAmt();
                    DecreaseInterval();
                    timer = CurrentInterval;
                }
                else
                {
                    timer -= Time.unscaledDeltaTime;
                }
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left && !mouseIsDown)
            {
                mouseIsDown = true;

                // start the new interval
                CurrentInterval = MAX_INTER;
                timer = CurrentInterval;

                ChangeAmt();
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                mouseIsDown = false;
            }
        }

        private void ChangeAmt()
        {
            if (increment)
            {
                shopToHandle.IncrementAmt();
            }
            else
            {
                shopToHandle.DecrementAmt();
            }
        }

        /// <summary>
        ///     Uses a decaying function to gradually lower the amount the current interval will be decreased by.
        /// </summary>
        private void DecreaseInterval()
        {
            // decaying function
            CurrentInterval *= Mathf.Pow(1 - INTER_DECR, 2);

            // clamp the value
            CurrentInterval = Mathf.Clamp(CurrentInterval, MIN_INTER, MAX_INTER);
        }
    }
}

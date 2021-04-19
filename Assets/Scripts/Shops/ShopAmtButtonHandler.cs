using UnityEngine;
using UnityEngine.EventSystems;

namespace FarmSim.Shops
{
    public class ShopAmtButtonHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private Shop shopToHandle;
        [SerializeField] private bool increment;

        private const float MAX_INTER = 0.3f;
        private const float MIN_INTER = 0.01f;
        private const float INTER_DECR = 0.1f;

        private float CurrentInterval;
        private float timer = 0;
        private bool isDown;

        private void Update()
        {
            if (isDown)
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
            if (eventData.button == PointerEventData.InputButton.Left && !isDown)
            {
                isDown = true;

                CurrentInterval = MAX_INTER;
                timer = CurrentInterval;

                ChangeAmt();
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                isDown = false;
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

        private void DecreaseInterval()
        {
            // decaying function
            CurrentInterval *= Mathf.Pow(1 - INTER_DECR, 2);

            // clamp the value
            CurrentInterval = Mathf.Clamp(CurrentInterval, MIN_INTER, MAX_INTER);
        }
    }
}

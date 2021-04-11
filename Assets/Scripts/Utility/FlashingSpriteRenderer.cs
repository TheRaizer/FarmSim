using UnityEngine;

namespace FarmSim.Utility
{
    /// <class name="FlashingSpriteRenderer">
    ///     <summary>
    ///         Manages the repeated flashing of some sprite.
    ///     </summary>
    /// </class>
    public class FlashingSpriteRenderer : MonoBehaviour
    {
        [SerializeField] private float flashInterval;
        private float timer;

        private SpriteRenderer spriteRenderer;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            timer += Time.deltaTime;

            if (timer >= flashInterval)
            {
                timer = 0;
                spriteRenderer.enabled = !spriteRenderer.enabled;
            }
        }
    }
}
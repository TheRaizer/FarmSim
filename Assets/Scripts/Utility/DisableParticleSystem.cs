using UnityEngine;

namespace FarmSim.Utility
{
    /// <class name="DisableParticleSystem">
    ///     <summary>
    ///         Disables a particle system once it has stopped playing.
    ///     </summary>
    /// </class>
    public class DisableParticleSystem : MonoBehaviour
    {
        private ParticleSystem particles;

        void Awake() => particles = GetComponent<ParticleSystem>();

        void Update()
        {
            DisableObjectWhenNotPlaying();
        }

        private void DisableObjectWhenNotPlaying()
        {
            if (particles.isStopped)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
using UnityEngine;

namespace FarmSim.Utility
{
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
using UnityEngine;

namespace FarmSim.Utility
{
    [System.Serializable]
    public class Sound
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public bool Loop { get; private set; }

        [SerializeField] private AudioClip clip;

        [Range(0f, 1f)] [SerializeField] private float volume = 1;
        [Range(.1f, 3f)] [SerializeField] private float pitch = 1;

        public AudioSource Source { get; private set; }

        public void SetSource(AudioSource _source)
        {
            Source = _source;
            Source.clip = clip;
            Source.volume = volume;
            Source.pitch = pitch;
            Source.loop = Loop;
        }
    }
}

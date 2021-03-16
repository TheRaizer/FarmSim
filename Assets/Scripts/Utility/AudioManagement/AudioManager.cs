using System.Collections.Generic;
using UnityEngine;

namespace FarmSim.Utility
{
    public class AudioManager : Singleton<AudioManager>
    {
        [SerializeField] private List<Sound> sounds;

        private readonly Dictionary<string, Sound> soundsDict = new Dictionary<string, Sound>();

        private void Awake()
        {
            sounds.ForEach(sound => { 
                sound.SetSource(gameObject.AddComponent<AudioSource>());
                soundsDict.Add(sound.Name, sound);
            });
        }

        public void Play(string name)
        {
            if (!soundsDict.ContainsKey(name))
            {
                Debug.LogError($"Sound with name {name} does not exist.");
                return;
            }

            Sound sound = soundsDict[name];
            sound.Source.Play();
        }
    }
}
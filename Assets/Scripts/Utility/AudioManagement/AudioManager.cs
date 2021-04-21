using System.Collections.Generic;
using UnityEngine;

namespace FarmSim.Utility
{
    /// <class name="AudioManager">
    ///     <summary>
    ///         Manages the sounds that will be played during the game.
    ///     </summary>
    /// </class>
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private List<Sound> sounds;

        private readonly Dictionary<string, Sound> soundsDict = new Dictionary<string, Sound>();

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            sounds.ForEach(sound =>
            {
                sound.SetSource(gameObject.AddComponent<AudioSource>());
                soundsDict.Add(sound.Name, sound);
            });
        }

        public void Play(string name)
        {
            if (!soundsDict.ContainsKey(name))
            {
                Debug.LogWarning($"Sound with name {name} does not exist.");
                return;
            }

            Sound sound = soundsDict[name];
            sound.Source.Play();
        }
    }
}
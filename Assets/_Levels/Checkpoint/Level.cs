using Randolph.Core;
using UnityEngine;

namespace Randolph.Levels {
    public class Level : MonoBehaviour {

        [HideInInspector] public bool areaFold = true;

        public AudioClip levelMusic;
        
        // TODO: Allow this to be overwritten for specific areas
        // TODO: Smooth music transitions when it happens

        AudioSource audioSource;

        void Start() {
            AudioPlayer.audioPlayer.SetGlobalMusic(levelMusic);
        }

    }
}

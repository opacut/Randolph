using System.Collections;
using System.Collections.Generic;
using Com.LuisPedroFonseca.ProCamera2D;
using Randolph.Characters;
using Randolph.Levels;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Randolph.Core {
    public class AudioPlayer : MonoBehaviour {

        public static AudioPlayer audioPlayer;

        class SoundQueue {

            public Queue<AudioClip> SoundsQueue { get; set; }
            public Coroutine PlayingCoroutine { get; set; }

            public SoundQueue(IEnumerable<AudioClip> sounds) {
                SoundsQueue = new Queue<AudioClip>(sounds);
                PlayingCoroutine = null;
            }

        }

        [SerializeField] AudioSource musicSource;
        [SerializeField] AudioSource soundSource; // For global sounds (objective, death…)
        [SerializeField] Vector2 pitchRange = new Vector2(0.95f, 1.05f);
        [SerializeField, HideInInspector] AudioListener audioListener;

        [SerializeField] AudioPlayerMode audioPlayerMode = AudioPlayerMode.Rooms;

        public enum AudioPlayerMode {

            Global,
            Rooms,
            Local

        };

        const float SpatialBlendGlobal = 0f;
        const float SpatialBlendLocal = 1f;

        Dictionary<AudioSource, SoundQueue> playingSounds = new Dictionary<AudioSource, SoundQueue>();

        Transform player;
        ProCamera2DRooms cameraRooms;
        [SerializeField] Area currentArea;

        void Awake() {
            //! Pass Audio Player between levels; destroy excess ones
            DontDestroyOnLoad(this);

            if (FindObjectsOfType(GetType()).Length > 1) {
                Destroy(gameObject);
            } else {
                audioPlayer = this;
            }

            LevelManager.OnNewLevel += OnNewLevel;
        }

        void LateUpdate() {
            //! Required as long as the AudioPlayer also carries the listener
            if (player.hasChanged) transform.position = player.position;
        }

        float GetSpatialBlend() {
            return (audioPlayerMode == AudioPlayerMode.Global) ? SpatialBlendGlobal : SpatialBlendLocal;
        }

        void OnNewLevel(Scene scene, PlayerController playerController) {
            if (musicSource == null || soundSource == null) {
                Debug.LogWarning($"One of the audio sources of <b>{gameObject.name}</b> is null.", gameObject);
                return;
            }
            player = playerController.transform;
            soundSource.spatialBlend = GetSpatialBlend();
            playingSounds.Clear();
            playingSounds.Add(soundSource, new SoundQueue(new Queue<AudioClip>()));
            cameraRooms = FindObjectOfType<ProCamera2DRooms>();

            if (cameraRooms && (audioPlayerMode == AudioPlayerMode.Rooms)) {
                currentArea = null;
                cameraRooms.OnFinishedTransition.AddListener(RoomChange);
                currentArea?.SetAreaSpatialBlend(GetSpatialBlend());
            }
        }

        /// <summary>An event triggered on transition between rooms.</summary>
        /// <param name="newRoomIndex">The index of the room being entered.</param>
        /// <param name="previousRoomIndex">The index of the previous room or -1 if there wasn't any.</param>
        void RoomChange(int newRoomIndex, int previousRoomIndex) {
            currentArea?.SetAreaSpatialBlend(SpatialBlendLocal);
            currentArea = Area.GetArea(newRoomIndex);
            currentArea?.SetAreaSpatialBlend(SpatialBlendGlobal);
        }

        public AudioSource AddAudioSource(GameObject audioGameObject) {
            var audioSource = audioGameObject.AddComponent<AudioSource>();
            //! Settings
            audioSource.playOnAwake = true;
            audioSource.spatialBlend = GetSpatialBlend();
            audioSource.loop = false;
            return audioSource;
        }

        /// <summary>Plays multiple local sounds, one after each other with (optionally) a randomly altered pitch.</summary>
        public void PlayLocalSounds(AudioSource audioSource, bool randomPitch = true, params AudioClip[] sounds) {            
            if (audioPlayerMode == AudioPlayerMode.Rooms) {
                //! No current area or an outside audio source
                if (!audioSource.transform.IsChildOf(transform) && (currentArea == null || !currentArea.Contains(audioSource.gameObject))) {                    
                    return;
                } else {
                    //! Play sound
                    // Spatial blend should already be set during room transition
                }
            } else {
                audioSource.spatialBlend = GetSpatialBlend();
            }

            if (!playingSounds.ContainsKey(audioSource)) {
                var soundsQueue = new Queue<AudioClip>();
                foreach (AudioClip sound in sounds) {
                    if (sound) soundsQueue.Enqueue(sound);
                }
                playingSounds.Add(audioSource, new SoundQueue(soundsQueue));
            } else {
                var soundsQueue = playingSounds[audioSource].SoundsQueue;
                foreach (AudioClip sound in sounds) {
                    if (sound) soundsQueue.Enqueue(sound);
                }
            }

            if (playingSounds[audioSource].PlayingCoroutine == null) {
                playingSounds[audioSource].PlayingCoroutine = StartCoroutine(PlayQueue(audioSource, playingSounds[audioSource], randomPitch));
            }
        }

        /// <summary>The method processing all sounds played by the central audio source.</summary>        
        IEnumerator PlayQueue(AudioSource audioSource, SoundQueue soundQueue, bool randomPitch) {
            float initialSoundPitch = audioSource.pitch;
            while (soundQueue.SoundsQueue.Count > 0) {
                AudioClip sound = soundQueue.SoundsQueue.Dequeue();
                if (randomPitch) audioSource.pitch = Random.Range(pitchRange.x, pitchRange.y);
                audioSource.clip = sound;
                audioSource.Play();
                yield return new WaitForSecondsRealtime(sound.length);
            }
            if (randomPitch) audioSource.pitch = initialSoundPitch;
            soundQueue.PlayingCoroutine = null;
        }

        // Global sounds → use the AudioPlayer's audio source
        // Local sounds → use the current object's audio source (add it first with AudioPlayer.audioPlayer.AddAudioListener)

        #region Overloaded methods

        /// <summary>Plays a local sound after the current one finishes playing.</summary>
        public void PlayLocalSound(AudioSource audioSource, AudioClip sound, bool randomPitch = true) {
            PlayLocalSounds(audioSource, true, sound);
        }

        /// <summary>Plays a random local sound out of the given sounds with (optionally) a randomly altered pitch.</summary>
        public void PlayRandomLocalSound(AudioSource audioSource, bool randomPitch = true, params AudioClip[] sounds) {
            int randomIndex = Random.Range(0, sounds.Length);
            PlayLocalSound(audioSource, sounds[randomIndex]);
        }

        /// <summary>Plays a global sound using the <see cref="soundSource"/> after the current one finishes playing.</summary>
        public void PlayGlobalSound(AudioClip sound, bool randomPitch = true) {
            PlayLocalSound(soundSource, sound, randomPitch);
        }

        /// <summary>Plays a random global sound out of the given sounds using the <see cref="soundSource"/> with (optionally) a randomly altered pitch.</summary>
        public void PlayRandomGlobalSound(bool randomPitch = true, params AudioClip[] sounds) {
            PlayRandomLocalSound(soundSource, randomPitch, sounds);
        }

        /// <summary>Plays multiple global sounds, one after each other using the <see cref="soundSource"/> with (optionally) a randomly altered pitch.</summary>
        public void PlayGlobalSounds(bool randomPitch = true, params AudioClip[] sounds) {
            PlayLocalSounds(soundSource, randomPitch, sounds);
        }

        #endregion

    }
}

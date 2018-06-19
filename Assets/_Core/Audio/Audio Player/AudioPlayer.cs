using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Com.LuisPedroFonseca.ProCamera2D;
using Randolph.Levels;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Randolph.Core {
    public class AudioPlayer : MonoBehaviour {

        public static AudioPlayer audioPlayer; // TODO: Property; create automatically if null
        public const string VolumeKey = "Volume";

        public static float GlobalVolume {
            get {
                if (!PlayerPrefs.HasKey(VolumeKey)) PlayerPrefs.SetFloat(VolumeKey, 1f);
                return PlayerPrefs.GetFloat(VolumeKey);
            }
            set { SetGlobalVolume(value); }
        }

        /// <summary>A small class for queuing sounds one after each other.</summary>
        class SoundQueue {

            public Queue<AudioClip> SoundsQueue { get; }
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
            if (FindObjectsOfType(GetType()).Length > 1) {
                Destroy(gameObject);
            } else {
                audioPlayer = this;
                DontDestroyOnLoad(this);
                SceneManager.sceneUnloaded += StopMusicOnLevelEnd;
                LevelManager.OnNewLevel += OnNewLevel; // Only works in levels with a player
            }
        }

        /// <summary>Makes sure the music doesn't continue to play when moving between levels (e.g. to Menu).</summary>
        void StopMusicOnLevelEnd(Scene scene) {
            // TODO: Edit if music should continue between levels
            StopGlobalMusic();
            musicSource.clip = null;
        }

        /// <summary>Stops the global music from playing and clears the music queue. Sounds are unaffected.</summary>
        void StopGlobalMusic() {
            //! Not used → music is not set via a queue to play (to enable looping)
            SoundQueue music;
            if (playingSounds.TryGetValue(musicSource, out music)) {
                StopCoroutine(music.PlayingCoroutine);
                playingSounds[musicSource].SoundsQueue.Clear();
            }
        }

        void LateUpdate() {
            //! Required as long as the AudioPlayer also carries the listener
            if (player != null && player.hasChanged) transform.position = player.position;
        }

        /// <summary>Sets the <see cref="AudioListener"/>'s volume to a specified value and saves it in <see cref="PlayerPrefs"/>.</summary>
        public static void SetGlobalVolume(float volume) {
            AudioListener.volume = volume;
            PlayerPrefs.SetFloat(VolumeKey, volume); // remember the state
        }

        /// <summary>Mutes music and sound when the game is paused.</summary>
        public static void PauseGlobalVolume() {
            PlayerPrefs.SetFloat(VolumeKey, AudioListener.volume); // remember the state
            AudioListener.volume = 0f;
        }

        /// <summary>Resumes music and sound volume after the game is resumed.</summary>
        public static void ResumeGlobalVolume() {
            AudioListener.volume = GlobalVolume; // old value or 1f
        }

        float GetSpatialBlend() {
            return (audioPlayerMode == AudioPlayerMode.Global) ? SpatialBlendGlobal : SpatialBlendLocal;
        }

        void OnNewLevel(Scene scene) {
            if (musicSource == null || soundSource == null) {
                Debug.LogWarning($"One of the audio sources of <b>{gameObject.name}</b> is null.", gameObject);
                return;
            }

            // TODO: Get global music
            // TODO: Add audio sources

            player = Constants.Randolph.transform; // Set once a level to avoid checking multiple times
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

        void OnDestroy() {
            LevelManager.OnNewLevel -= OnNewLevel;
        }

        /// <summary>An event triggered on transition between rooms.</summary>
        /// <param name="newRoomIndex">The index of the room being entered.</param>
        /// <param name="previousRoomIndex">The index of the previous room or -1 if there wasn't any.</param>
        void RoomChange(int newRoomIndex, int previousRoomIndex) {
            currentArea?.SetAreaSpatialBlend(SpatialBlendLocal);
            currentArea = Area.GetArea(newRoomIndex);
            currentArea?.SetAreaSpatialBlend(SpatialBlendGlobal);
        }

        /// <summary>Adds a new <see cref="AudioSource"/> component to the specified <see cref="GameObject"/>.</summary>
        /// <param name="audioGameObject">The object which should be the source of sounds.</param>
        public AudioSource AddAudioSource(GameObject audioGameObject) {
            var audioSource = audioGameObject.AddComponent<AudioSource>();
            //! Settings
            audioSource.playOnAwake = true;
            audioSource.spatialBlend = GetSpatialBlend();
            audioSource.loop = false;
            return audioSource;
        }

        /// <summary>Sets the global music clip and makes sure it loops.</summary>
        /// <param name="clip"></param>
        public void SetGlobalMusic(AudioClip clip) {
            musicSource.Stop();            
            musicSource.loop = true;
            musicSource.clip = clip;
            musicSource.Play();
        }

        /// <summary>Plays multiple local sounds, one after each other with (optionally) a randomly altered pitch.</summary>
        public void PlayLocalSounds(AudioSource audioSource, bool randomPitch = true, float volume = 1.0f, params AudioClip[] sounds) {
            if (audioPlayerMode == AudioPlayerMode.Rooms) {
                if (!audioSource.transform.IsChildOf(transform) && (currentArea == null || !currentArea.Contains(audioSource.gameObject))) {
                    //! Not a global sound, player sound or no current area or an outside audio source --> block
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
                Queue<AudioClip> soundsQueue = playingSounds[audioSource].SoundsQueue;
                foreach (AudioClip sound in sounds) {
                    // TODO: Ok?
                    //? Allows only one instance of each sound in the queue                    
                    if (sound && !soundsQueue.Contains(sound)) soundsQueue.Enqueue(sound);
                }
            }

            if (playingSounds[audioSource].PlayingCoroutine == null) {
                playingSounds[audioSource].PlayingCoroutine = StartCoroutine(PlayQueue(audioSource, playingSounds[audioSource], randomPitch, volume));
            }
        }

        /// <summary>The coroutine processing all sounds played by the central audio source.</summary>        
        IEnumerator PlayQueue(AudioSource audioSource, SoundQueue soundQueue, bool randomPitch, float volume = 1.0f) {
            float initialSoundPitch = audioSource.pitch;
            while (soundQueue.SoundsQueue.Count > 0) {
                AudioClip sound = soundQueue.SoundsQueue.Dequeue();
                if (randomPitch) audioSource.pitch = Random.Range(pitchRange.x, pitchRange.y);
                audioSource.volume = volume.Clamp01();
                audioSource.clip = sound;
                audioSource.Play();
                yield return new WaitForSecondsRealtime(sound.length);
                audioSource.clip = null;
            }
            if (randomPitch) audioSource.pitch = initialSoundPitch;
            soundQueue.PlayingCoroutine = null;
        }

        // Global sounds → use the AudioPlayer's audio source (e.g. should always play, even when an object becomes inactive)
        // Local sounds → use the current object's audio source (add it first with AudioPlayer.audioPlayer.AddAudioListener)

        #region Overloaded methods

        /// <summary>Plays a local sound after the current one finishes playing.</summary>
        public void PlayLocalSound(AudioSource audioSource, AudioClip sound, bool randomPitch = true, float volume = 1.0f) {
            PlayLocalSounds(audioSource, true, volume, sound);
        }

        /// <summary>Plays a random local sound out of the given sounds with (optionally) a randomly altered pitch.</summary>
        public void PlayRandomLocalSound(AudioSource audioSource, bool randomPitch = true, float volume = 1.0f, params AudioClip[] sounds) {
            int randomIndex = Random.Range(0, sounds.Length);
            PlayLocalSound(audioSource, sounds[randomIndex], randomPitch, volume);
        }

        /// <summary>Plays a global sound using the <see cref="soundSource"/> after the current one finishes playing.</summary>
        public void PlayGlobalSound(AudioClip sound, bool randomPitch = true, float volume = 1.0f) {
            if (soundSource.clip != sound) PlayLocalSound(soundSource, sound, randomPitch, volume);
        }

        /// <summary>Plays a random global sound out of the given sounds using the <see cref="soundSource"/> with (optionally) a randomly altered pitch.</summary>
        public void PlayRandomGlobalSound(bool randomPitch = true, float volume = 1.0f, params AudioClip[] sounds) {
            sounds = sounds.Where(sound => soundSource.clip != sound).ToArray(); // avoid playing the same thing multiple times
            PlayRandomLocalSound(soundSource, randomPitch, volume, sounds);
        }

        /// <summary>Plays multiple global sounds, one after each other using the <see cref="soundSource"/> with (optionally) a randomly altered pitch.</summary>
        public void PlayGlobalSounds(bool randomPitch = true, float volume = 1.0f, params AudioClip[] sounds) {
            sounds = sounds.Where(sound => soundSource.clip != sound).ToArray(); // avoid playing the same thing multiple times
            PlayLocalSounds(soundSource, randomPitch, volume, sounds);
        }

        /// <summary>Clears the sound queue and plays the given sound immediately.</summary>
        //public void PlayGlobalSoundNow(bool randomPitch = true, float volume = 1.0f, para1)

        #endregion

    }
}

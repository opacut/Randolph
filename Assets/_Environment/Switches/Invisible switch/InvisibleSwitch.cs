using Randolph.Core;
using Randolph.Interactable;
using Randolph.Levels;
using UnityEngine;

public class InvisibleSwitch : MonoBehaviour, IRestartable {
    private AudioSource audioSource;
    [SerializeField] private Bats bats;
    [SerializeField] private bool On;
    [SerializeField] private AudioClip thumpSound;

    public void Restart() { On = false; }

    private void Awake() { audioSource = AudioPlayer.audioPlayer.AddAudioSource(gameObject); }

    private void Flip(bool active) { On = active; }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.GetComponent<Boulder>()) {
            AudioPlayer.audioPlayer.PlayLocalSound(audioSource, thumpSound);
            Flip(true);
            bats.StartMoving();
        }
    }
}

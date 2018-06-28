using System;
using Randolph.Core;
using Randolph.Interactable;
using Randolph.Levels;
using UnityEngine;


// TODO: Refactor
public class FruitInvisibleSwitch : RestartableBase {
    public bool Activated;
    private AudioSource audioSource;
    public GameObject fruitHolder;
    private Collider2D fruitHolderCollider;

    [SerializeField]
    private AudioClip thumpSound;

    public void Awake() {
        audioSource = AudioPlayer.audioPlayer.AddAudioSource(gameObject);
    }

    public void SetPickable(bool value) {
        fruitHolderCollider.enabled = value;
    }

    private void Start() {
        fruitHolderCollider = fruitHolder.GetComponent<Collider2D>();
    }

    public void OnTriggerEnter2D(Collider2D collision) {
        if (!collision.GetComponent<Boulder>()) {
            return;
        }

        AudioPlayer.audioPlayer.PlayLocalSound(audioSource, thumpSound);
        SetPickable(false);
    }

    // TODO Implement
    #region IRestartable
    public override void Restart() {
        base.Restart();
        SetPickable(true);
    }
    #endregion
}

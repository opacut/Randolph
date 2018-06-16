﻿using Randolph.Core;
using Randolph.Interactable;
using Randolph.Levels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleSwitch : MonoBehaviour, IRestartable
{
    [SerializeField] AudioClip thumpSound;
    AudioSource audioSource;
    [SerializeField] bool On;
    [SerializeField] Bats bats;

    void Awake()
    {
        audioSource = AudioPlayer.audioPlayer.AddAudioSource(gameObject);
    }

    public void Restart()
    {
        On = false;
    }

    void Flip(bool active)
    {
        On = active;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Boulder>())
        {
            Flip(true);
            bats.StartMoving();
        }
    }
}

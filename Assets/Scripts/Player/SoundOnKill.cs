using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SoundOnKill : MonoBehaviour
{
    public static Action<ulong, ulong> OnKill;
    [SerializeField] AudioClip[] sounds;
    AudioSource myAudioSource;

    // Start is called before the first frame update
    void Start()
    {
        myAudioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        OnKill += OnKillSound;
    }

    private void OnDisable()
    {
        OnKill -= OnKillSound;
    }

    [ClientRpc]
    private void OnKillSound(ulong killerId, ulong deadId)
    {
        AudioClip clip = sounds[UnityEngine.Random.Range(0, sounds.Length)];
        myAudioSource.PlayOneShot(clip);
    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SoundOnKill : NetworkBehaviour
{
    public static Action<ulong, ulong> OnKill;
    [SerializeField] private AudioClip[] sounds;
    [SerializeField] private AudioSource myAudioSource;

    private void OnEnable()
    {
        OnKill += OnKillSoundServerRpc;
    }

    private void OnDisable()
    {
        OnKill -= OnKillSoundServerRpc;
    }
    
    private void Start()
    {
        if (!IsOwner)
        {
            enabled = false;
            
            // Disabled should unsubscribe the function but it doesn't work, so do it here
            OnKill -= OnKillSoundServerRpc;
        }

        myAudioSource.name = $"MyIdIs {OwnerClientId}";
    }

    [ServerRpc]
    private void OnKillSoundServerRpc(ulong killerId, ulong deadId)
    {
        GameObject PlayerObject = NetworkManager.ConnectedClients[killerId].PlayerObject.gameObject;
        PlayerObject.GetComponentInChildren<SoundOnKill>().PlaySoundClientRpc();
    }

    [ClientRpc]
    public void PlaySoundClientRpc()
    {
        if (!IsOwner)
            return;
        
        AudioClip clip = sounds[UnityEngine.Random.Range(0, sounds.Length)];
        myAudioSource.PlayOneShot(clip);
    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjects;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using Unity.VisualScripting;
using UnityEngine;

public class VacuumScript : NetworkBehaviour
{
    [SerializeField] private GameEventSO _vacuumEvent;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (NetworkManager.Singleton.IsServer)
                TriggerOnDestroy();
            else
                TriggerOnDestroyServerRPC();
        }
    }

    private void TriggerOnDestroy()
    {
        Destroy(gameObject);
    }
    
    [ServerRpc(RequireOwnership = false)]
    private void TriggerOnDestroyServerRPC()
    {
        Destroy(gameObject);
    }
    
    private void OnDestroy()
    {
        _vacuumEvent.Raise();
    }
}

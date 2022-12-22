using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class KillFeedScript : NetworkBehaviour
{
    public static Action<ulong, ulong> OnKill;

    [SerializeField] private float feedDuration = 5f;
    [SerializeField] private PlayerUsernameList _playerUsernames;
    [SerializeField] private Transform _parent;
    [SerializeField] private GameObject _killPfb;

    private void OnEnable()
    {
        OnKill += OnKillFeed;
    }

    private void OnDisable()
    {
        OnKill -= OnKillFeed;
    }

    private void OnKillFeed(ulong killerId, ulong deadId)
    {
        var killerName = _playerUsernames.GetNameViaId(killerId);
        var deadName = _playerUsernames.GetNameViaId(deadId);

        ShowKillFeedClientRpc(killerName, deadName);
    }

    [ClientRpc]
    private void ShowKillFeedClientRpc(string killerName, string deadName)
    {
        var rowScript = Instantiate(_killPfb, _parent).GetComponent<KillFeedRow>();
        
        rowScript.SetKillerText(killerName);
        rowScript.SetDeadText(deadName);

        Destroy(rowScript.gameObject, feedDuration);
    }
}

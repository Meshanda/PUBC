using System;
using System.Collections;
using System.Collections.Generic;
using Network;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class LobbySpawner : MonoBehaviour
{
    [SerializeField] private List<LobbyPlayer> _players;
    [SerializeField] private PlayerUsernameList _playerUsernameList;

    private void OnEnable()
    {
        LobbyEvents.OnLobbyUpdated += OnLobbyUpdated;
    }

    private void OnDisable()
    {
        LobbyEvents.OnLobbyUpdated -= OnLobbyUpdated;
    }

    private void OnLobbyUpdated(Lobby lobby)
    {
        List<LobbyPlayerData> playerDatas = GameLobbyManager.Instance.GetPlayers();

        _playerUsernameList.playersNames.Clear();

        for (int i = 0; i < playerDatas.Count; i++)
        {
            PlayerName playerName = new PlayerName();
            playerName.username = playerDatas[i].Gamertag;
            playerName.clientId = (ulong)i;
            _playerUsernameList.playersNames.Add(playerName);
            _players[i].SetData(playerDatas[i]);
        }
    }
}

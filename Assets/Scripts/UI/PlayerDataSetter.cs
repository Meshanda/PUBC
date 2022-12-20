using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Network;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

public class PlayerDataSetter : NetworkBehaviour
{
    [SerializeField] private TextMeshPro _nameLabel;
    [SerializeField] private Transform _modelTransform;
    [SerializeField] private SkinSelectionData _skinSelectionData;

    private NetworkVariable<FixedString64Bytes> _name = new NetworkVariable<FixedString64Bytes>();
    private bool _dataSet;
    private LobbyPlayerData _lobbyData;
    private ulong _clientId;

    public List<GameObject> players;
    public ulong ClientId => _clientId;

    private PlayerDataList _playerDatas = new PlayerDataList()
    {
        playerData1 = new PlayerData() {clientId = 999, pseudo = "", skinIndex = -1},
        playerData2 = new PlayerData() {clientId = 999, pseudo = "", skinIndex = -1},
        playerData3 = new PlayerData() {clientId = 999, pseudo = "", skinIndex = -1},
        playerData4 = new PlayerData() {clientId = 999, pseudo = "", skinIndex = -1}
    };

    public struct PlayerDataList : INetworkSerializable
    {
        public PlayerData playerData1;
        public PlayerData playerData2;
        public PlayerData playerData3;
        public PlayerData playerData4;
        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref playerData1);
            serializer.SerializeValue(ref playerData2);
            serializer.SerializeValue(ref playerData3);
            serializer.SerializeValue(ref playerData4);
        }
    }
    public struct PlayerData : INetworkSerializable  
    {
        public ulong clientId;
        public FixedString64Bytes pseudo;
        public int skinIndex;
        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref clientId);
            serializer.SerializeValue(ref pseudo);
            serializer.SerializeValue(ref skinIndex);
        }
    }
    
    public override void OnNetworkSpawn()
    {
        UpdateNamesServerRpc();
        _clientId = NetworkManager.Singleton.LocalClientId;
        
        base.OnNetworkSpawn();
    }

    [ServerRpc(RequireOwnership = false)]
    private void UpdateNamesServerRpc()
    {
        foreach (var networkClient in NetworkManager.Singleton.ConnectedClientsList)
        {
            networkClient.PlayerObject.GetComponent<PlayerDataSetter>().RefreshPlayerDataClientRpc();
        }
    }

    [ClientRpc]
    private void RefreshPlayerDataClientRpc()
    {
        if (_lobbyData != null) 
            SetPlayerData(_lobbyData);
    }

    public void SetPlayerData(LobbyPlayerData lobbyData)
    {
        if (_dataSet) return;

        _lobbyData = lobbyData;
        SetDataServerRpc(lobbyData.Gamertag, lobbyData.SkinIndex, new ServerRpcParams());

        _dataSet = true;
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetDataServerRpc(string newName, int skinIndex, ServerRpcParams receiveParams) 
    {
        _name.Value = newName;
        
        var data = new PlayerData
        {
            clientId = receiveParams.Receive.SenderClientId,
            pseudo = newName,
            skinIndex = skinIndex
        };

        if (_playerDatas.playerData1.clientId == 999)
            _playerDatas.playerData1 = data;
        else if (_playerDatas.playerData2.clientId == 999)
            _playerDatas.playerData2 = data;
        else if (_playerDatas.playerData3.clientId == 999)
            _playerDatas.playerData3 = data;
        else if (_playerDatas.playerData4.clientId == 999)
            _playerDatas.playerData4 = data;
        
        SendDataClientRpc(_playerDatas);
    }

    [ClientRpc]
    private void SendDataClientRpc(PlayerDataList playerDatas)
    {
        players = GameObject.FindGameObjectsWithTag("Player").ToList();
        players.ToList().ForEach(player =>
        {
            if (player.name.Equals("Collider"))
            {
                players.Remove(player);
            }
        });
        
        var playerDatasList = new List<PlayerData>
        {
            playerDatas.playerData1,
            playerDatas.playerData2,
            playerDatas.playerData3,
            playerDatas.playerData4
        };

        foreach (var playerGo in players)
        {
            foreach (var playerData in playerDatasList)
            {
                Debug.Log($"go Network Client Id: {playerGo.GetComponent<PlayerDataSetter>().ClientId}  PlayerData Client Id {playerData.clientId}");
                
                if (playerGo.GetComponent<PlayerDataSetter>().ClientId == playerData.clientId)
                {
                    playerGo.GetComponent<PlayerDataSetter>().SetData(playerData);
                }
            }
        }
    }

    private void SetData(PlayerData data)
    {
        SetName(data.pseudo.ToString());
    }

    public void SetName(string newName)
    {
        _nameLabel.text = newName;
    }
}

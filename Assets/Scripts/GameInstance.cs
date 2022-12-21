using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.FPS.Game;
using UnityEngine.Events;

public class GameInstance : NetworkBehaviour
{
    public static GameInstance instance;
    public NetworkList<PlayerKillAmount> playersKills;

    private void Awake()
    {
        playersKills = new NetworkList<PlayerKillAmount>();
    }
    private void Start()
    {
        if (instance == null) 
            instance = this;
        else
            Destroy(this);
    }
    public override void OnNetworkSpawn()
    {
        if(IsServer || IsHost)
        {
            foreach (var clientId in NetworkManager.ConnectedClientsIds)
            {
                AddPlayer(clientId);
            }

            NetworkManager.OnServerStarted += OnServerStarted;
            NetworkManager.OnClientConnectedCallback += AddPlayer;

            GameManager.instance.OnGameRestart += OnGameRestart;
        }
    }
    public void AddKill(ulong clientID, int killAmountToAdd = 1)
    {
        PlayerKillAmount playerKills = GetPlayerKillAmount(clientID);
        playerKills.killsAmount += killAmountToAdd;
        ReplacePlayerKillAmount(clientID, playerKills);
    }
    public void AddKill(NetworkClient client, int killAmountToAdd = 1)
    {
        AddKill(client.ClientId, killAmountToAdd);
    }
    private void OnServerStarted()
    {
        NetworkManager.OnClientDisconnectCallback += RemovePlayer;
    }
    public void AddPlayer(ulong clientId)
    {
        PlayerKillAmount newPlayerKillAmount = new PlayerKillAmount();
        newPlayerKillAmount.clientId = clientId;
        newPlayerKillAmount.killsAmount = 0;
        playersKills.Add(newPlayerKillAmount);

        NetworkManager.ConnectedClients[clientId].PlayerObject.GetComponent<Health>().OnDie += OnPlayerKill;
    }
    private void RemovePlayer(ulong clientId)
    {
        PlayerKillAmount playerKillAmount = GetPlayerKillAmount(clientId);
        playersKills.Remove(playerKillAmount);
    }
    private PlayerKillAmount GetPlayerKillAmount(ulong clientId)
    {
        foreach (var playerKills in playersKills)
        {
            if (playerKills.clientId == clientId) return playerKills;
        }
        throw new System.Exception($"No player found with the id {clientId}");
    }
    private void ReplacePlayerKillAmount(ulong clientId, PlayerKillAmount pka)
    {
        for (int i = 0; i < playersKills.Count; i++)
        {
            if (playersKills[i].clientId == clientId)
            {
                playersKills[i] = pka;
                break;
            }
        }
        OrderPlayerKills();
    }
    public void OnPlayerKill(ulong killerClientId)
    {
        AddKill(killerClientId);
    }
    public void OnGameRestart()
    {
        playersKills = new NetworkList<PlayerKillAmount>();
    }
    private void OrderPlayerKills()
    {
        int n = playersKills.Count;
        for (int i = 0; i < n - 1; i++)
            for (int j = 0; j < n - i - 1; j++)
                if (playersKills[j].killsAmount > playersKills[j + 1].killsAmount)
                {
                    // swap temp and arr[i]
                    PlayerKillAmount temp = playersKills[j];
                    playersKills[j] = playersKills[j + 1];
                    playersKills[j + 1] = temp;
                }
        foreach (var playerKills in playersKills)
        {
            Debug.Log($"client {playerKills.clientId} have {playerKills.killsAmount} kills");
        }
    }
}

public struct PlayerKillAmount : INetworkSerializable, System.IEquatable<PlayerKillAmount>
{
    public ulong clientId;
    public int killsAmount;

    public bool Equals(PlayerKillAmount other)
    {
        return clientId == other.clientId && killsAmount == other.killsAmount;
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        if (serializer.IsReader)
        {
            var reader = serializer.GetFastBufferReader();
            reader.ReadValueSafe(out clientId);
            reader.ReadValueSafe(out killsAmount);
        }
        else
        {
            var writer = serializer.GetFastBufferWriter();
            writer.WriteValueSafe(clientId);
            writer.WriteValueSafe(killsAmount);
        }
    }
}
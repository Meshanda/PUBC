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

        //Here plug the kill event listener
        //NetworkManager.ConnectedClients[clientId].PlayerObject.GetComponent<Health>().OnDie += OnPlayerKillServerRpc;
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
    }
    [ServerRpc]
    private void OnPlayerKillServerRpc(ulong killerClientId)
    {
        AddKill(killerClientId);
    }
    private void Update()
    {
        foreach (var playerKill in playersKills)
        {
            Debug.Log($"player with the id: {playerKill.clientId} have {playerKill.killsAmount} kills");
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
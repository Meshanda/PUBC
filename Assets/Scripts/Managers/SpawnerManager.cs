using System;
using System.Collections;
using System.Collections.Generic;
using Network;
using Unity.FPS.Game;
using Unity.FPS.UI;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnerManager : NetworkBehaviour
{
    [SerializeField] private PlayerUsernameList usernameSo;
    public GameObject prefab;
    public Terrain terrain;
    public float yOffset = 1f;
    public float respawnInvincibilityTime = 3f;

    private float terrainWidth;
    private float terrainLength;

    private float xTerrainPos;
    private float zTerrainPos;

    private void Awake()
    {
        if(!IsServer && !IsHost)
        {
            enabled = false;
        }
        //Get terrain size
        terrainWidth = terrain.terrainData.size.x / 2;
        terrainLength = terrain.terrainData.size.z / 2;

        //Get terrain position
        xTerrainPos = terrain.transform.position.x + terrain.terrainData.size.x / 4;
        zTerrainPos = terrain.transform.position.z+ terrain.terrainData.size.z / 4;

        foreach (var client in NetworkManager.ConnectedClients)
        {
            RespawnPlayer(client.Key);
        }

        NetworkManager.OnClientConnectedCallback += RespawnPlayer;
        NetworkManager.OnClientConnectedCallback += UpdateSoServerRpc;
    }
    
    public override void OnNetworkSpawn()
    {
        Debug.Log("IsServer:" + IsServer);
        Debug.Log("IsHost:" + IsHost);
        Debug.Log("IsClient:" + IsClient);

        if (IsServer) AddHostToSo();
        if (IsClient && !IsHost) {
            Debug.Log("SendingToServer");
            AddClientToServerRpc(NetworkManager.Singleton.LocalClientId,
            GameLobbyManager.Instance.GetLocalLobbyPlayerData.Gamertag);}
        
    }

    [ServerRpc(RequireOwnership = false)]
    private void AddClientToServerRpc(ulong id, string pseudo)
    {
        Debug.Log("AddClientToServer");
        usernameSo.playersNames.Add(new PlayerName
        {
            username = pseudo,
            clientId = id
        });
    }

    private void AddHostToSo()
    {
        Debug.Log("AddHost");
        usernameSo.playersNames.Clear();
        
        usernameSo.playersNames.Add(new PlayerName
        {
            username = GameLobbyManager.Instance.GetLocalLobbyPlayerData.Gamertag,
            clientId = NetworkManager.Singleton.LocalClientId
        });
    }

    [ServerRpc]
    private void UpdateSoServerRpc(ulong id)
    {
        Debug.Log("SendListToClients");

        ClearSoClientRpc();

        foreach (var player in usernameSo.playersNames)
        {
            UpdateSoClientRpc(player.username, player.clientId);
        }
    }
    

    [ClientRpc]
    private void UpdateSoClientRpc(string pseudo, ulong id)
    {
        if (IsHost) return;
        
        Debug.Log("AddEachClient");
        usernameSo.playersNames.Add(new PlayerName
        {
            username = pseudo,
            clientId = id
        });    
    }
    
    [ClientRpc]
    private void ClearSoClientRpc()
    {
        if (IsHost) return;
        
        Debug.Log("ClearClientSo");
        usernameSo.playersNames.Clear();
    }


    public void RespawnPlayer(ulong ownerId)
    {
        
        //Generate random x,z,y position on the terrain
        float randX = Random.Range(xTerrainPos, xTerrainPos + terrainWidth);
        float randZ = Random.Range(zTerrainPos, zTerrainPos + terrainLength);
        float yVal = Terrain.activeTerrain.SampleHeight(new Vector3(randX, 0, randZ));

        //Apply Offset if needed
        yVal = yVal + yOffset;

        //Generate the Prefab on the generated position
        GameObject playerGO = (GameObject)Instantiate(prefab, new Vector3(randX, yVal, randZ), Quaternion.identity);

        playerGO.GetComponent<NetworkObject>().SpawnAsPlayerObject(ownerId);
        playerGO.GetComponent<Health>().OnDie += OnPlayerDied;

        StartCoroutine(InvincibilityOnRespawn(playerGO));
    }

    private void OnPlayerDied(ulong killerId, ulong deadClientId)
    {
        RespawnPlayer(deadClientId);
        GameInstance.Instance.OnPlayerKill(killerId, deadClientId);

        SoundOnKill.OnKill?.Invoke(killerId, deadClientId);
        KillFeedScript.OnKill?.Invoke(killerId, deadClientId);
    }

    private IEnumerator InvincibilityOnRespawn(GameObject playerGO)
    {
        Health playerHealth = playerGO.GetComponent<Health>();
        playerHealth.Invincible.Value = true;

        yield return new WaitForSeconds(respawnInvincibilityTime);

        playerHealth.Invincible.Value = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.FPS.Game;
using Unity.Netcode;
using UnityEngine;

public class SpawnerManager : NetworkBehaviour
{
    public GameObject prefab;
    public Terrain terrain;
    public float yOffset = 1f;

    private float terrainWidth;
    private float terrainLength;

    private float xTerrainPos;
    private float zTerrainPos;

    private void Awake()
    {
        if(!IsServer && !IsHost) enabled = false;
        //Get terrain size
        terrainWidth = terrain.terrainData.size.x;
        terrainLength = terrain.terrainData.size.z;

        //Get terrain position
        xTerrainPos = terrain.transform.position.x;
        zTerrainPos = terrain.transform.position.z;

        foreach (var client in NetworkManager.ConnectedClients)
        {
            RespawnPlayer(client.Key);
        }
        NetworkManager.OnClientConnectedCallback += RespawnPlayer;
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
    }

    private void OnPlayerDied(ulong killerId, ulong deadClientId)
    {
        RespawnPlayer(deadClientId);
    }
}

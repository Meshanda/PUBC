using System;
using System.Collections;
using System.Collections.Generic;
using Unity.FPS.Game;
using Unity.FPS.UI;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

public class PowerUpManager : NetworkBehaviour
{
    [SerializeField] private NetworkObject[] prefabPowerUp;
    [SerializeField] private Terrain terrain;
    [SerializeField] private float yOffset = 1f;
    [SerializeField] private float TimeBetweenPowerUpSpawn;
    

    private float terrainWidth;
    private float terrainLength;

    private float xTerrainPos;
    private float zTerrainPos;

    private void Awake()
    {
        //Get terrain size
        terrainWidth = terrain.terrainData.size.x / 2;
        terrainLength = terrain.terrainData.size.z / 2;

        //Get terrain position
        xTerrainPos = terrain.transform.position.x + terrain.terrainData.size.x / 4;
        zTerrainPos = terrain.transform.position.z + terrain.terrainData.size.z / 4;
    }
    
    private void Start()
    {
        if (IsServer)
        {
            StartCoroutine(SpawnPowerUps());
        }
        
    }

    private IEnumerator SpawnPowerUps()
    {
        while (true)
        {
           SpawnPowerUpServerRpc();

           yield return new WaitForSeconds(TimeBetweenPowerUpSpawn);
        }
    }
    
    [ServerRpc]
    public void SpawnPowerUpServerRpc()
    {
        //Generate random x,z,y position on the terrain
        float randX = Random.Range(xTerrainPos, xTerrainPos + terrainWidth);
        float randZ = Random.Range(zTerrainPos, zTerrainPos + terrainLength);
        float yVal = Terrain.activeTerrain.SampleHeight(new Vector3(randX, 0, randZ));

        //Apply Offset if needed
        yVal = yVal + yOffset;

        //Generate the Prefab on the generated position
        NetworkObject powerUp = Instantiate(SelectRandomPowerUp(), new Vector3(randX, yVal, randZ), Quaternion.identity, transform);

        powerUp.Spawn();
    }

    private NetworkObject SelectRandomPowerUp()
    {
        return prefabPowerUp[Random.Range(0, prefabPowerUp.Length)];
    }
}

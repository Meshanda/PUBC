using System.Collections;
using System.Collections.Generic;
using ScriptableObjects;
using Unity.Netcode;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject EnemyToSpawn;
    [SerializeField] private float TimeBetweenSpawn;
    [SerializeField] private GenericLootTableSO GLTSO;
    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, 300, 300));
        if (!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer)
        {
            StartButtons();
        }
        else
        {
            StatusLabels();

            StartSpawning();

            DamageEnemy();
            
            ItemSpawnTest();
        }
        GUILayout.EndArea();
    }

    static void StartButtons()
    {
        if (GUILayout.Button("Host")) NetworkManager.Singleton.StartHost();
        if (GUILayout.Button("Client")) NetworkManager.Singleton.StartClient();
        if (GUILayout.Button("Server")) NetworkManager.Singleton.StartServer();
    }

    static void StatusLabels()
    {
        var mode = NetworkManager.Singleton.IsHost ?
            "Host" : NetworkManager.Singleton.IsServer ? "Server" : "Client";

        GUILayout.Label("Transport: " +
            NetworkManager.Singleton.NetworkConfig.NetworkTransport.GetType().Name);
        GUILayout.Label("Mode: " + mode);
    }

    void StartSpawning()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            if (GUILayout.Button("StartSpawn"))
            {
                StopCoroutine(SpawnEnemy());
                StartCoroutine(SpawnEnemy());
            }
        }
    }

    void DamageEnemy()
    {
        if (GUILayout.Button("Damage Enemy"))
        {
            GameObject Target = GameObject.FindGameObjectWithTag("Enemy");
            Target.GetComponent<EnemyScript>().Damage(10);
        }
    }

    void ItemSpawnTest()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            if (GUILayout.Button("Spawn Items"))
            {
                for (int i = 0; i < 100; i++)
                {
                    Death();
                }
            }
        }
    }
    
    void Death()
    {
        GenericItemSO GISO =  GLTSO.DroppedItem();
        if (GISO)
        {
            GameObject test = Instantiate(GISO.GetItemPrefab(), GetRandomPositionOnPlane(), Quaternion.identity);
            test.GetComponent<NetworkObject>().Spawn();
        }
        BaseXPItem BXPI = GLTSO.DroppedXP();
        if (BXPI)
        {
            GameObject test = Instantiate(BXPI.GetItemPrefab(), GetRandomPositionOnPlane(), Quaternion.identity);
            test.GetComponent<NetworkObject>().Spawn();
        }
    }
    
    static Vector3 GetRandomPositionOnPlane()
    {
        return new Vector3(Random.Range(-10f, 10f), 1f, Random.Range(-10f, 10f));
    }
    IEnumerator SpawnEnemy()
    {
        //while (true)
        //{
            GameObject Enemy = Instantiate(EnemyToSpawn, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
            Enemy.GetComponent<NetworkObject>().Spawn();
            yield return new WaitForSeconds(TimeBetweenSpawn);
        //}
    }
}

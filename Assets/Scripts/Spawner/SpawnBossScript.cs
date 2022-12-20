using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SpawnBossScript : MonoBehaviour
{
    [SerializeField] GameObject _bossPrefab;

    public void SpawnBoss()
    {
        GameObject gameObject = Instantiate(_bossPrefab);
        gameObject.GetComponent<NetworkObject>().Spawn();
    }
}

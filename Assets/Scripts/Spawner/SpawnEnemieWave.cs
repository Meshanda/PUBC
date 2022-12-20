using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class SpawnEnemieWave : MonoBehaviour
{
    [SerializeField] private List<tableEnemiStat> table;
    [SerializeField] private float _betweenObject;
    private List<GameObject> _toSpawn;
    
    public int  SpawnPool(int budget, List<GameObject> players) //add what gameobject to spawn with the current budget
    {

        _toSpawn = FillToSpawn(budget);
        Spawn(players);

        return _toSpawn.Count;
    }

    private List<GameObject> FillToSpawn(int budget) 
    {
        List<GameObject> list = new List<GameObject>();
        for (int i = table.Count - 1; i >= 0; i--)
        {
            if (budget >= table[i].cost)
            {
                list.Add(table[i].enemie);
                list.AddRange(FillToSpawn(budget - table[i].cost));
                break;
            }
        }
        return list;
    }

    private void Spawn( List<GameObject> players )
    {
        //Debug.Log(nbreWave);
        for (int i = 0; i < _toSpawn.Count; i++)
        {
            Vector3 offSet = Vector3.zero;
            int direction = 1;

            if (i % 2 == 0)
                direction = -1;

            if (i != 0)
            {
                offSet = transform.right;
            }

            offSet = offSet * direction * Mathf.RoundToInt((i / 2f) + 0.1f) * _betweenObject ;
            GameObject go = Instantiate(_toSpawn[i], transform.position + offSet, transform.rotation, transform);
            go.GetComponent<NetworkObject>().Spawn();


        }
    }

}

[Serializable]
public struct tableEnemiStat 
{
    public GameObject enemie;
    public float hp;
    public float speed;
    public int cost;

}

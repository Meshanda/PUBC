using Network;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SpawnEnemieSpawner : MonoBehaviour
{
    [SerializeField] private GameObjectListVariable _players;
    [SerializeField] private GameObject _enemySpawner;

    [SerializeField] private float _distanceFromPlayer;
    [SerializeField] private float _timeInit;
    private float _time = 0;

    [SerializeField, Range(0, 1)] private float _vectorOffset;

    private Vector3 _opposingVectorDebug = Vector3.zero;
    private int _playerIndexDebug = 0;

    private int _budget = 1;

    [SerializeField] private ScriptableObjects.Variables.FloatVariable _nbrSpawnned;
    [SerializeField] private float _maxSpawn;

    // Start is called before the first frame update
    private void Start()
    {
        _nbrSpawnned.value = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (_players == null || _players.GetGos().Count <= GameLobbyManager.Instance.GetPlayers().Count-1 ||  !NetworkManager.Singleton.IsServer)
        {

            return;
        }

        _time -= Time.deltaTime;

        if (_time > 0 || !NetworkManager.Singleton.IsServer)
            return;

        _time = _timeInit;

        Spawn();
    }

    private void Spawn()
    {
        if (_players == null|| _players.GetGos().Count <= 0 || _nbrSpawnned.value > _maxSpawn)
        {
            return;
        }
        GameObject[] players = _players.GetGos().ToArray();

        for (int j = 0; j < players.Length; j++)
        {
            int playerIndex = j;

            if (players[playerIndex] == null)
                Debug.LogError("_playerNull WTF");

            Vector3 opposingVector = Vector3.zero;

            for (int i = 0; i < players.Length; i++)
            {
                if (i == playerIndex || players[i] == null)
                    continue;

                opposingVector += (players[i].transform.position - players[playerIndex].transform.position).normalized;
            }

            opposingVector = -opposingVector.normalized;

            opposingVector.z += Random.Range(-0.75f, 0.75f);
            opposingVector.x += Random.Range(-0.75f, 0.75f);

            opposingVector = opposingVector.normalized;

            GameObject go = Instantiate(_enemySpawner, players[playerIndex].transform.position + opposingVector * _distanceFromPlayer, Quaternion.identity);
            _nbrSpawnned.value += go.GetComponent<SpawnEnemieWave>().SpawnPool(_budget,new List<GameObject>(players));
            _opposingVectorDebug = opposingVector;

            _playerIndexDebug = playerIndex;

            go.transform.LookAt(players[j].transform);
        }
        nextBudget();
    }

    private void nextBudget()
    {
        _budget = (int)(_budget + 1);
    }

}
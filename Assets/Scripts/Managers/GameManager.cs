using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using ScriptableObjects.Variables;
using UnityEngine.Events;
using System;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance;
    public Action OnGameRestart;
    [SerializeField]private IntVariable _maxWinKill;
    private bool _hasGameEnded;

    public bool GameEnded => _hasGameEnded; 

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(this);
    }
    public override void OnNetworkSpawn()
    {
        GameInstance.Instance.playersKills.OnListChanged += OnPlayersKillsChanged;
    }

    private void OnPlayersKillsChanged(NetworkListEvent<PlayerKillAmount> changedPlayersKills)
    {
        if (changedPlayersKills.Value.killsAmount >= _maxWinKill.value)
        {
            Debug.Log($"Player with the id: {changedPlayersKills.Value.clientId} won !");
            StartCoroutine(GameFinished());
        }
    }

    public IEnumerator GameFinished(bool byTime = false)
    {
        _hasGameEnded = true;
        EndManager.GameEnd?.Invoke();
        
        if (byTime)
            Debug.Log("Game finished by time");
        else
            Debug.Log("Game is finished by kills");

        yield return new WaitForSeconds(3f);

        RestartGame();
    }

    private void RestartGame()
    {
        OnGameRestart?.Invoke();
    }
}

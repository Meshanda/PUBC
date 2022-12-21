using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using ScriptableObjects.Variables;
using UnityEngine.Events;

public class GameManager : NetworkBehaviour
{
    public static GameManager instance;
    public UnityAction OnGameEnd;
    public UnityAction OnGameRestart;
    [SerializeField]private IntVariable _maxWinKill;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(this);
    }
    public override void OnNetworkSpawn()
    {
        GameInstance.instance.playersKills.OnListChanged += OnPlayersKillsChanged;
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
        OnGameEnd.Invoke();
        if (byTime)
            Debug.Log("Game finished by time");
        else
            Debug.Log("Game is finished by kills");

        yield return new WaitForSeconds(3f);

        RestartGame();
    }

    private void RestartGame()
    {
        OnGameRestart.Invoke();
    }
}

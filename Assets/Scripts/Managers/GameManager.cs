using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using ScriptableObjects.Variables;

public class GameManager : NetworkBehaviour
{
    public static GameManager instance;
    [SerializeField]private IntVariable _maxWinKill;

    private void Start()
    {
        if (instance == null)
            instance = this;
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
            GameFinished();
        }
    }

    public void GameFinished(bool byTime = false)
    {
        if (byTime)
            Debug.Log("Game finished by time");
        else
            Debug.Log("Game is finished by kills");
    }
}

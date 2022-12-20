using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerListDeath :MonoBehaviour
{
    [SerializeField] private GameObjectListVariable _players;
    [SerializeField] private GameEventSO _endGame;
    // Start is called before the first frame update

    private void Update( )  // Fuck it
    { 
        foreach(GameObject player in _players.gos) 
        {
            if( player.active == false )
                return;
        }
        _endGame.Raise();
        Debug.Log("End");
    }
}

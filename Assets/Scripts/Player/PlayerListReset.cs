using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerListReset : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObjectListVariable _players;
    void Start()
    {
        //if (!NetworkManager.Singleton.IsServer)
        //    return;
        //_players.SetGos(new List<GameObject>());
    }
}

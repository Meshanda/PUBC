using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SpawnNetworkObject : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (!NetworkManager.Singleton.IsHost) return;
        
        var networkObject = GetComponent<NetworkObject>();
        if (!networkObject.IsSpawned)
            networkObject.Spawn(true);
    }
}

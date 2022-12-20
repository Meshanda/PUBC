using System;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

public class NetcodeConnection : NetworkBehaviour
{
    void Start()
    {
        if (!NetworkManager.Singleton.IsHost && false)
        {
            var transport = NetworkManager.Singleton.gameObject.GetComponent<UnityTransport>();
            transport.ConnectionData.Address = RelayManager.Instance.Ip;
            transport.ConnectionData.Port = Convert.ToUInt16(RelayManager.Instance.Port);
            
            transport.SetRelayServerData(RelayManager.Instance.Ip, 
                Convert.ToUInt16(RelayManager.Instance.Port),
                RelayManager.Instance.GetAllocationIdByte,
                RelayManager.Instance.ClientKey,
                RelayManager.Instance.GetConnectionDataByte,
                RelayManager.Instance.HostConnectionData
            );
            
            NetworkManager.Singleton.StartClient();
        }
    }

    private void Update()
    {
        if (!NetworkManager.Singleton.IsHost) return;
        
        if (Input.GetKeyDown(KeyCode.A))
        {
            UpdateClientRpc();
        }
    }

     [ClientRpc]
    private void UpdateClientRpc()
    {
        Debug.Log("COUCOU");
    }
}

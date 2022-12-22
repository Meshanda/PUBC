using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class TeamPicker : MonoBehaviour
{
    public void SelectTeam(int teamIndex)
    {
        ulong localClientID = NetworkManager.Singleton.LocalClientId;

        if(!NetworkManager.Singleton.ConnectedClients.TryGetValue(localClientID, out NetworkClient networkClient)) { return; }

        if (!networkClient.PlayerObject.TryGetComponent<TeamPlayer>(out TeamPlayer teamPlayer)) { return; }

        teamPlayer.SetTeamServerRpc((byte) teamIndex);
    }
}

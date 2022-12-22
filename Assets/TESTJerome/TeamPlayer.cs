using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class TeamPlayer : NetworkBehaviour
{
    [SerializeField] private Renderer TeamColourRenderer;
    [SerializeField] private Color[] teamColours;

    private NetworkVariable<byte> teamIndex = new NetworkVariable<byte>();

    [ServerRpc]
    public void SetTeamServerRpc(byte newTeamIndex)
    {
        if(newTeamIndex > 3) { return; }

        teamIndex.Value = newTeamIndex;
    }

    private void OnEnable()
    {
        teamIndex.OnValueChanged += OnTeamChanged;
    }
    
    private void OnDisable()
    {
        teamIndex.OnValueChanged -= OnTeamChanged;
    }

    private void OnTeamChanged(byte oldTeamIndex, byte newTeamIndex)
    {
        if(!IsClient) { return; }

        Debug.Log("Color Changed");
        TeamColourRenderer.material.SetColor("_BaseColor", teamColours[newTeamIndex]);
    }
}

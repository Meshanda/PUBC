using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndManager : NetworkBehaviour
{
    public static Action GameEnd;
    [SerializeField] private List<GameObject> elementsToHide;
    [SerializeField] private List<GameObject> elementsToShow;

    private void OnEnable()
    {
        GameEnd += OnGameEndClientRpc;
    }

    private void OnDisable()
    {
        GameEnd -= OnGameEndClientRpc;
    }

    [ClientRpc]
    private void OnGameEndClientRpc()
    {
        Debug.Log("################### PD");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        elementsToHide.ForEach(e => e.gameObject.SetActive(false));
        elementsToShow.ForEach(e => e.gameObject.SetActive(true));
        EndScoreboard.FillScoreboard?.Invoke();
    }
    
    public void ClickMenu()
    {
        if (IsServer)
        { 
            ClientHandlerClientRPC();
        }
        
        SceneManager.LoadScene("MainMenu");
    }

    [ClientRpc]
    private void ClientHandlerClientRPC()
    {
        SceneManager.LoadScene("MainMenu");
    }
}

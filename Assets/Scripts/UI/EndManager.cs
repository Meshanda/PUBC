using System;
using System.Collections;
using System.Collections.Generic;
using Unity.FPS.UI;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
    using UnityEditor;
#endif

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
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        // haha yes, no time to make it beautiful
        NetworkManager.LocalClient.PlayerObject.GetComponentInChildren<WeaponHUDManager>().gameObject.SetActive(false);
        
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
        
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#endif
        Application.Quit();
    }

    [ClientRpc]
    private void ClientHandlerClientRPC()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#endif
        Application.Quit();
    }
}

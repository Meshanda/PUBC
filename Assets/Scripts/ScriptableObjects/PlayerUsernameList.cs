using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Player username list")]
public class PlayerUsernameList : ScriptableObject
{
    public List<PlayerName> playersNames = new List<PlayerName>();

    public string GetNameViaId(ulong id)
    {
        var pseudo = playersNames.Find((pn) => pn.clientId == id).username;

        return pseudo ?? "RandomPlayer";
    }

    public void UpdateNameViaId(ulong id, string pseudo)
    {
        playersNames.Find(pn => pn.clientId == id).username = pseudo;
    }
}

[System.Serializable]
public class PlayerName
{
    public string username;
    public ulong clientId;
}
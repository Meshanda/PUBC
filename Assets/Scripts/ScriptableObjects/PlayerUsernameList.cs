using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Player username list")]
public class PlayerUsernameList : ScriptableObject
{
    public List<PlayerName> playersNames = new List<PlayerName>();

    public string GetNameViaId(ulong id)
    {
        return playersNames.Find((pn) => pn.clientId == id).username;
    }
}

[System.Serializable]
public class PlayerName
{
    public string username;
    public ulong clientId;
}
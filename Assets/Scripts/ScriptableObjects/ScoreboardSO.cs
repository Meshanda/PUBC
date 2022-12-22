using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Scoreboard")]
public class ScoreboardSO : ScriptableObject
{
    public List<ScoreboardPlayer> players;
}

[System.Serializable]
public class ScoreboardPlayer
{
    public string playerName;
    public int numberOfKills;
}

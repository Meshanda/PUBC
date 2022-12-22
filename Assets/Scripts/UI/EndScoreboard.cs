using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EndScoreboard : MonoBehaviour
{
    public static Action FillScoreboard;
    
    [SerializeField] private GameObject _rowPfb;
    [SerializeField] private Transform _parent;
    [SerializeField] private ScoreboardSO _scoreboardData;

    private void OnEnable()
    {
        FillScoreboard += OnFillScoreboard;
    }

    private void OnDisable()
    {
        FillScoreboard -= OnFillScoreboard;
    }

    private void OnFillScoreboard()
    {
        foreach (var player in _scoreboardData.players)
        {
            var rowScript = Instantiate(_rowPfb, _parent).GetComponent<RowScript>();
            rowScript.SetKillText(player.numberOfKills.ToString());
            rowScript.SetNameText(player.playerName);

            rowScript.transform.localScale *= 2;
        }
    }

}

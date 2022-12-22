using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjects.Variables;
using TMPro;
using UnityEngine;

public class ScoreboardScript : MonoBehaviour
{
    [SerializeField] private GameObject _rowPfb;
    [SerializeField] private Transform _panelParent;
    [SerializeField] private ScoreboardSO _scoreboardData;

    [SerializeField] private TextMeshProUGUI _killText;
    [SerializeField] private IntVariable _killGoal;

    private void Start()
    {
        _killText.text = "Kill goal: " + _killGoal.value;
    }

    private void Update()
    {
        if (GameManager.Instance.GameEnded) return;
        
        if (!Input.GetKey(KeyCode.Tab))
        {
            _panelParent.gameObject.SetActive(false);
            return;
        }
        
        _panelParent.gameObject.SetActive(true);
        ClearScoreboard();
        FillScoreboard();
    }

    private void ClearScoreboard()
    {
        foreach (var child in _panelParent.GetComponentsInChildren<RowScript>())
        {
            Destroy(child.gameObject);
        }
    }

    private void FillScoreboard()
    {
        foreach (var player in _scoreboardData.players)
        {
            var rowScript = Instantiate(_rowPfb, _panelParent).GetComponent<RowScript>();
            rowScript.SetKillText(player.numberOfKills.ToString());
            rowScript.SetNameText(player.playerName);
        }
    }
}

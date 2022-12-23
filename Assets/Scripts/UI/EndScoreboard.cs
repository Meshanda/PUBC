using System;
using UnityEngine;

public class EndScoreboard : MonoBehaviour
{
    public static Action FillScoreboard;
    
    [SerializeField] private GameObject _rowPfb;
    [SerializeField] private Transform _parent;
    [SerializeField] private ScoreboardSO _scoreboardData;

    private bool _fill;
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
        _fill = true;
        
    }

    private void Update()
    {
        if (!_fill) return;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        ClearScoreboard();
        Fill();
    }

    private void Fill()
    {
        foreach (var player in _scoreboardData.players)
        {
            var rowScript = Instantiate(_rowPfb, _parent).GetComponent<RowScript>();
            rowScript.SetKillText(player.numberOfKills.ToString());
            rowScript.SetNameText(player.playerName);

            rowScript.transform.localScale *= 2;
        }
    }

    private void ClearScoreboard()
    {
        foreach (var child in _parent.GetComponentsInChildren<RowScript>())
        {
            Destroy(child.gameObject);
        }
    }
}

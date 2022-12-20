using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjects.Variables;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [Header("Overlay")] 
    [SerializeField] private FloatVariable _lvlSo;
    [SerializeField] private FloatVariable _xpSo;
    [SerializeField] private FloatVariable _xpMaxSo;
    [SerializeField] private GameObject _overlayCanvas;

    [Header("xp bar")]
    [SerializeField] private Image _fillBar;
    [SerializeField] private TextMeshProUGUI _levelLabel;

    private void Start()
    {
        UIManager.Instance.SetCrosshairCursor();
    }

    private void Update()
    {
        _fillBar.fillAmount = _xpSo.value / _xpMaxSo.value;
        _levelLabel.text = "Level " + _lvlSo.value;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjects.Variables;
using TMPro;
using UnityEngine;

public class TimerScript : MonoBehaviour
{
    [SerializeField] private FloatVariable _timerFloat;
    [SerializeField] private TextMeshProUGUI _timerText;

    private void Update()
    {
        var min = Mathf.Floor(_timerFloat.value / 60);
        var sec = _timerFloat.value % 60;

        _timerText.text = $"{min:00}:{sec:00}";
    }
}

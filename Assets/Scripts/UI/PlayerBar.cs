using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBar : MonoBehaviour
{
    [SerializeField] private PlayerHpSetter _hpComponent;
    [SerializeField] private Image _img;

    private void Update()
    {
        _img.fillAmount = _hpComponent.CurrentHp / _hpComponent.MaxHp;
    }
}

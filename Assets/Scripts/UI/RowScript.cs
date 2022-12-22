using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RowScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private TextMeshProUGUI _kill;

    public void SetKillText(string txt)
    {
        _kill.text = txt;
    }
    public void SetNameText(string txt)
    {
        _name.text = txt;
    }
}

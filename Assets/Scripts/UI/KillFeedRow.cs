using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KillFeedRow : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _killerText;
    [SerializeField] private TextMeshProUGUI _deadText;

    public void SetKillerText(string txt)
    {
        _killerText.text = txt;
    }
    public void SetDeadText(string txt)
    {
        _deadText.text = txt;
    }
}

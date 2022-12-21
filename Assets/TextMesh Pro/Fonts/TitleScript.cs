using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TitleScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private float _textSizeMin;
    [SerializeField] private float _textSizeMax;
    [SerializeField] private float _speed;

    void Start()
    {
        StartCoroutine(Routine());
    }

    private IEnumerator Routine()
    {
        do
        {
            _text.fontSize += _speed;

            yield return null;
        } 
        while (_text.fontSize <= _textSizeMax);
        
        do
        {
            _text.fontSize -= _speed;

            yield return null;
        }
        while (_text.fontSize >= _textSizeMin) ;
        
        StartCoroutine(Routine());
    }
}

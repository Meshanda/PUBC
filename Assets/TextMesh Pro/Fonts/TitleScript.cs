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

    public float Speed
    {
        get => _speed;
        set => _speed = value;
    }

    public float TextMinSize
    {
        get => _textSizeMin;
        set => _textSizeMin = value;
    }
    public float TextMaxSize
    {
        get => _textSizeMax;
        set => _textSizeMax = value;
    }

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

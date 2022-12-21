using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollButtonScript : MonoBehaviour
{
    [SerializeField] private RawImage _rawImage;
    [SerializeField] private float _speed;
    private void Update()
    {
        var rawImageUVRect = _rawImage.uvRect;
        rawImageUVRect.x -= _speed;

        if (rawImageUVRect.x < -1000) rawImageUVRect.x = 0;

        _rawImage.uvRect = rawImageUVRect;
    }
}

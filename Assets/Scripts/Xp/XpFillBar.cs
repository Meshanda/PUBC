using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class XpFillBar : MonoBehaviour
{
    [SerializeField] private ScriptableObjects.Variables.FloatVariable _xpSo, _xpThreshSo;

    [SerializeField] private  Image image;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        image.fillAmount =_xpSo.value / _xpThreshSo.value;
    }
}

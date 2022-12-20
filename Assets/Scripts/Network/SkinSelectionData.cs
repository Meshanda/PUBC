using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Data/SkinSelectionData", fileName = "SkinSelectionData")]
public class SkinSelectionData : ScriptableObject
{
    public List<SkinInfo> Skins;
}

[Serializable]
public struct SkinInfo
{
    public string SkinName;
    public GameObject Model;
}
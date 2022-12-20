using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Data/MapSelectionData", fileName = "MapSelectionData")]
public class MapSelectionData : ScriptableObject
{
    public List<MapInfo> Maps;
}

[Serializable]
public struct MapInfo
{
    public string MapName;
    public string SceneName;
}

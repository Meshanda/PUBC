using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Random = UnityEngine.Random;

public class ChangeSkinEnemy : MonoBehaviour
{
    [SerializeField] private List<GameObject> _skinList;

    private int _randomSkinChosen;
    // Start is called before the first frame update
    void Start()
    {
        _randomSkinChosen = Random.Range(0, _skinList.Count);
        _skinList[_randomSkinChosen].active = true;
    }
}

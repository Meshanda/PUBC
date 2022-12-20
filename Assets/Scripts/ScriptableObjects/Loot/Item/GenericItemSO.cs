using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects
{
    public class GenericItemSO : ScriptableObject
    { 
        [SerializeField] private GameObject _itemPrefab;
        public GameObject GetItemPrefab()
        {
            return _itemPrefab;
        }
    }
}


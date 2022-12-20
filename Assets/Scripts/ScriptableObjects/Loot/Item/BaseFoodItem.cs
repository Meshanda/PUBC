using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Loot/Item/BaseFoodItem", fileName = "New BaseFoodItem")]
    public class BaseFoodItem : GenericItemSO
    {
        [SerializeField] private float _foodValue;
    }
}


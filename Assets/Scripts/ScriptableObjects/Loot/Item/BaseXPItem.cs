using System.Collections;
using System.Collections.Generic;
using ScriptableObjects;
using ScriptableObjects.Variables;
using Unity.VisualScripting;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Loot/Item/BaseXPItem", fileName = "New BaseXPItem")]
    public class BaseXPItem : GenericItemSO
    {
        [SerializeField] public float xpValue;
    }
}


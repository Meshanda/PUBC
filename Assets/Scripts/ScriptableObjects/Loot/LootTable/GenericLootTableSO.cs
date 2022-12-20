using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

[Serializable]
public class LootTableItemInfo
{
    public GenericItemSO Item;
    public float DropRate;
}

[Serializable]
public class LooTableXPInfo
{
    public BaseXPItem XP;
    public float DropRate;
}

namespace ScriptableObjects
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Loot/LootTable", fileName = "New BaseLootTable")]
    public class GenericLootTableSO : ScriptableObject
    {
        [Tooltip("The chance to drop all item doesn't have to be equal to 1")] 
        [SerializeField] private LootTableItemInfo[] _itemLootTable;
        [Tooltip("The chance to drop all item have to be equal to 1")] 
        [SerializeField] private LooTableXPInfo[] _xpLootTable;
        public GenericItemSO DroppedItem()
        {
            float diceRoll = Random.Range(0f, 1);
            foreach (LootTableItemInfo itemInfo in _itemLootTable)
            {
                if (itemInfo.DropRate >= diceRoll)
                {
                    return itemInfo.Item;
                }

                diceRoll -= itemInfo.DropRate;
            }
            return null;
        }
        
        public  BaseXPItem DroppedXP()
        {
            float diceRoll = Random.Range(0f, 1);
            foreach (LooTableXPInfo xpInfo in _xpLootTable)
            {
                if (xpInfo.DropRate >= diceRoll)
                {
                    return xpInfo.XP;
                }
                diceRoll -= xpInfo.DropRate;
            }
            return null;
        }
    }
}


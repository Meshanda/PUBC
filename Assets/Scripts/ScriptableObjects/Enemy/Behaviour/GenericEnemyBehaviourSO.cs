using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects
{
    public abstract class GenericEnemyBehaviourSO : ScriptableObject
    {
        public abstract GameObject ChoseTargettedPlayer(GameObject[] players, GameObject enemy);
    }
}


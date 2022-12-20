using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects.Variables
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Variables/String", fileName = "New StringVariable")]
    public class StringVariable : GenericVariableSO<string>
    { }
}

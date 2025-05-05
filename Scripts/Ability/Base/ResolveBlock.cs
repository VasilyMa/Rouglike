using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AbilitySystem
{
    [System.Serializable]
    public class ResolveBlock
    {
        [SerializeReference] public List<IAbilityEffect> Components;
    }
}

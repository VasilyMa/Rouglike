using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AbilitySystem
{
    [System.Serializable]
    public class BasicBlock
    {
        [SerializeReference] public List<IAbilityBaseComponent> BasicComponents;
    }
}

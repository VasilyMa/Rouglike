using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AbilitySystem
{
    [System.Serializable]
    public class InputBlock
    {
        public bool Pressing;
        [SerializeReference] public List<IAbilityComponent> Components;
    }
}


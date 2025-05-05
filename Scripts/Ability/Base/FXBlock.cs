using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AbilitySystem
{
    [System.Serializable]
    public class TimeLineBlock
    {
        public float Timer;
        [SerializeReference] public List<IAbilityComponent> FXComponents;
    }
}

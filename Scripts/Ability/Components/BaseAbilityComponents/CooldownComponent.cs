using AbilitySystem;
using UnityEngine;
using Leopotam.EcsLite;
using System;

namespace Client 
{
    struct CoolDownComponent : IAbilityBaseComponent
    {
        public Action<float, float> OnCooldown;
        public float CoolDownValue;
        [HideInInspector] public float CurrentCoolDownValue;

        public void Init(int entity, EcsWorld world)
        {
            ref var coolDownComp = ref world.GetPool<CoolDownComponent>().Add(entity);
            coolDownComp.CoolDownValue = CoolDownValue;
            coolDownComp.CurrentCoolDownValue = 0;
        }
    }
}
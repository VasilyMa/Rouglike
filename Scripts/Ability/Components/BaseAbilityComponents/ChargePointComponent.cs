using System;

using AbilitySystem;
using Leopotam.EcsLite;
using UnityEngine;

namespace Client {
    struct ChargePointComponent : IAbilityBaseComponent
    {
        public Action<int> OnChargePointChange;
        public int MaxChargeCount;
        [HideInInspector] public int CurrentChargeCount;
        public void Init(int entity, EcsWorld world)
        {
            ref var chargePointComp = ref world.GetPool<ChargePointComponent>().Add(entity);
            chargePointComp.MaxChargeCount = MaxChargeCount;
            chargePointComp.CurrentChargeCount = MaxChargeCount;
        }
    }
}
using AbilitySystem;
using Leopotam.EcsLite;
using UnityEngine;
using System.Collections.Generic;
namespace Client {
    struct ChargeComponent : IAbilityBaseComponent
    {
        public float ChargeMaxTime;
        [HideInInspector] public float CurrentChargeTimer;
        [HideInInspector, Range(0, 1)]public float CurrentCharge;
        [SerializeReference] public List<IFullChargeComponent> FullChargeComponents;

        //todo play any sound


        public void Init(int entity, EcsWorld world)
        {
            ref var chargeComp = ref world.GetPool<ChargeComponent>().Add(entity);
            chargeComp.ChargeMaxTime = ChargeMaxTime;
            chargeComp.CurrentChargeTimer = 0f;
            chargeComp.CurrentCharge = 0f;
            chargeComp.FullChargeComponents = new(FullChargeComponents);
        }
    }
}
using AbilitySystem;
using Leopotam.EcsLite;
using System.Collections.Generic;
using UnityEngine;

namespace Client {
    struct ChangeResolveMissileComponent : IAbilityMissileComponent
    {
        [SerializeReference] public List<IAbilityEffect> ResolveBlocks;
        [HideInInspector] public float charge;

        public void Invoke(int entity, EcsWorld world, float charge)
        {
            ref var changeResolve = ref world.GetPool<ChangeResolveMissileComponent>().Add(entity);
            changeResolve.charge = charge;
            changeResolve.ResolveBlocks = ResolveBlocks;
        }
    }
}
using AbilitySystem;

using Leopotam.EcsLite;
using System.Collections.Generic;
using UnityEngine;

namespace Client 
{
    struct TargetHimselfComponent : IAbilityComponent
    {

        public void Init()
        {

        }

        public void Invoke(int entityCaster,int abilityEntity,EcsWorld world, float charge = 1)
        {
            world.GetPool<TargetHimselfComponent>().Add(entityCaster);
        }

        public void Dispose(int entityCaster,int abilityEntity, EcsWorld world)
        {

        }

    }
    struct ResolveHimselfEffectComponent : IAbilityComponent
    {
        [SerializeReference] public List<IAbilityEffect> Effects;
        public void Dispose(int entityCaster,int abilityEntity, EcsWorld world)
        {
        }

        public void Init()
        {
        }

        public void Invoke(int entityCaster, int abilityEntity, EcsWorld world, float charge = 1)
        {
            EcsPool<ResolveHimselfEffectComponent> _pool = world.GetPool<ResolveHimselfEffectComponent>();
            ref var resolveComp=ref _pool.Add(entityCaster);
            resolveComp.Effects=Effects;
        }
    }
}
using AbilitySystem;
using Leopotam.EcsLite;
using System.Collections.Generic;
using UnityEngine;
namespace Client {
    struct ResolveOverlayToUnitComponent : IConditionComponent
    {
        [SerializeReference] public List<IConditionResolve> Resolve;

        public void Invoke(int entityCondition, EcsWorld world)
        {
            var _resolvePool = world.GetPool<ResolveOverlayToUnitComponent>();
            if (!_resolvePool.Has(entityCondition)) _resolvePool.Add(entityCondition);
            ref var resolveOverlayToUnitComp = ref _resolvePool.Get(entityCondition);
            resolveOverlayToUnitComp.Resolve = new List<IConditionResolve>(Resolve);
        }
    }
}
using AbilitySystem;
using Leopotam.EcsLite;
using System.Collections.Generic;
using UnityEngine;
namespace Client {
    struct ResolveOfAddingToMaxPointsComponent : IConditionComponent
    {
        [SerializeReference] public List<IConditionResolve> Resolve; 
        public void Invoke(int entityCondition, EcsWorld world)
        {
            var _resolvePool = world.GetPool<ResolveOfAddingToMaxPointsComponent>();
            if (!_resolvePool.Has(entityCondition)) _resolvePool.Add(entityCondition);
            ref var resolveComp = ref _resolvePool.Get(entityCondition);
            resolveComp.Resolve = new(Resolve);
        }
    }
}
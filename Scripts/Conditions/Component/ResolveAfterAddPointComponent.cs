using AbilitySystem;
using Leopotam.EcsLite;
using System.Collections.Generic;
using UnityEngine;
namespace Client {
    struct ResolveAfterAddPointComponent :IConditionComponent
    {
        [SerializeReference] public List<IConditionResolve> Resolve;
        public void Invoke(int entityCondition, EcsWorld world)
        {
            var _resolvePool = world.GetPool<ResolveAfterAddPointComponent>();
            if (!_resolvePool.Has(entityCondition)) _resolvePool.Add(entityCondition);
            ref var removePointComp = ref _resolvePool.Get(entityCondition);
            removePointComp.Resolve = new(Resolve);
        }
    }
}
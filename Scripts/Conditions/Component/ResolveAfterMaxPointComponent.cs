using AbilitySystem;
using JetBrains.Annotations;
using Leopotam.EcsLite;
using System.Collections.Generic;
using UnityEngine;

namespace Client {
    struct ResolveAfterMaxPointComponent :IConditionComponent {
        [SerializeReference] public List<IConditionResolve> Resolve;
        public void Invoke(int entityCondition, EcsWorld world)
        {
            var _resolvePool = world.GetPool<ResolveAfterMaxPointComponent>();
            if (!_resolvePool.Has(entityCondition)) _resolvePool.Add(entityCondition);
            ref var resoleComp = ref _resolvePool.Get(entityCondition);
            resoleComp.Resolve = new(Resolve);
        }
    }
}
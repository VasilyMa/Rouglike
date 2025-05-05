using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class ResolveAfterMaxPointSystem : MainEcsSystem {
        readonly EcsFilterInject<Inc<ResolveAfterMaxPointComponent, MaxPointConditionEvent,ConditionCompnent>> _filter;
        readonly EcsPoolInject<ResolveConditionEvent> _resolveConditionPool;
        readonly EcsPoolInject<ResolveAfterMaxPointComponent> _resolveAfterMaxPointPool;

        public override MainEcsSystem Clone()
        {
            return new ResolveAfterMaxPointSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach(var entity in _filter.Value)
            {
                ref var resolveAfterMaxPointComp = ref _resolveAfterMaxPointPool.Value.Get(entity);
                if(!_resolveConditionPool.Value.Has(entity)) _resolveConditionPool.Value.Add(entity).resolve = new();
                ref var resolveConditionComp = ref _resolveConditionPool.Value.Get(entity);
                resolveConditionComp.resolve.AddRange(resolveAfterMaxPointComp.Resolve);
            }
        }
    }
}
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class ResolveOverlayToUnitSystem : MainEcsSystem {
        readonly EcsFilterInject<Inc<ResolveOverlayToUnitComponent, ConditionOverlayToUnitEvent>> _filter;
        readonly EcsPoolInject<ResolveConditionEvent> _resolveConditionPool;
        readonly EcsPoolInject<ResolveOverlayToUnitComponent> _resolveOverlayToUnitPool;

        public override MainEcsSystem Clone()
        {
            return new ResolveOverlayToUnitSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach(var entity in _filter.Value)
            {
                ref var resolveOverlayComp = ref _resolveOverlayToUnitPool.Value.Get(entity);
                if (!_resolveConditionPool.Value.Has(entity)) _resolveConditionPool.Value.Add(entity).resolve = new();
                ref var resolveConditionComp = ref _resolveConditionPool.Value.Get(entity);
                resolveConditionComp.resolve.AddRange(resolveOverlayComp.Resolve);
            }
        }
    }
}
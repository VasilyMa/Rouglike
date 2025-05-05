using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class ResolveOfAddingToMaxPointsSystem : MainEcsSystem {
        readonly EcsFilterInject<Inc<ResolveOfAddingToMaxPointsComponent, AddPointConditionEvent, MaxPointConditionComponent>> _filter;
        readonly EcsPoolInject<ResolveOfAddingToMaxPointsComponent> _resolveOfAddingPool;
        readonly EcsPoolInject<ResolveConditionEvent> _resolveConditionPool;

        public override MainEcsSystem Clone()
        {
            return new ResolveOfAddingToMaxPointsSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach (var entity in _filter.Value)
            {
                ref var resolveOfAddingComp = ref _resolveOfAddingPool.Value.Get(entity);
                if (!_resolveConditionPool.Value.Has(entity)) _resolveConditionPool.Value.Add(entity).resolve = new();
                ref var resolveConditionComp = ref _resolveConditionPool.Value.Get(entity);
                resolveConditionComp.resolve.AddRange(resolveOfAddingComp.Resolve);
            }
        }
    }
}
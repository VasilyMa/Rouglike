using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class ResolveAfterAddPointSystem : MainEcsSystem {
        readonly EcsFilterInject<Inc<ResolveAfterAddPointComponent, AddPointConditionEvent>,Exc<MaxPointConditionComponent,MaxPointConditionEvent>> _filter;
        readonly EcsPoolInject<ResolveAfterAddPointComponent> _resolveAfterAddPointPool;
        readonly EcsPoolInject<ResolveConditionEvent> _resolveConditionPool;

        public override MainEcsSystem Clone()
        {
            return new ResolveAfterAddPointSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach(var entity in _filter.Value)
            {
                ref var resolveAfterAddPointComp = ref _resolveAfterAddPointPool.Value.Get(entity);
                if (!_resolveConditionPool.Value.Has(entity)) _resolveConditionPool.Value.Add(entity).resolve = new();
                ref var resolveConditionComp = ref _resolveConditionPool.Value.Get(entity);
                resolveConditionComp.resolve.AddRange(resolveAfterAddPointComp.Resolve);
            }
        }
    }
}
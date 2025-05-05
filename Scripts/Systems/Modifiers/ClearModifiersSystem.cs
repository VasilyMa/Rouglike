using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class ClearModifiersSystem : MainEcsSystem {
        readonly EcsWorldInject _world = default;
        readonly EcsFilterInject<Inc<ModifiersContainerChangesEvent, AbilityUnitComponent, PlayerComponent>> _filter = default;
        readonly EcsFilterInject<Inc<AbilityComponent, PlayerAbilityComponent>> _abilityFilter = default;
        readonly EcsPoolInject<ResolveBlocksAbilityComponent> _resolveBlockPool = default;
        public override MainEcsSystem Clone()
        {
            return new ClearModifiersSystem();
        }
        public override void Run (IEcsSystems systems) {
            foreach(var entity in _filter.Value)
            {
                foreach(var abilityEntity in _abilityFilter.Value)
                {
                    ref var resolveBlockComp = ref _resolveBlockPool.Value.Get(abilityEntity);
                    resolveBlockComp.Components = new (resolveBlockComp.OriginalComponents);
                }
            }
        }
    }
}
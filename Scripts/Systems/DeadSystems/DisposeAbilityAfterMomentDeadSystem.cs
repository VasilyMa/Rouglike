using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class DisposeAbilityAfterMomentDeadSystem : MainEcsSystem 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsFilterInject<Inc<MomentDeadEvent>> _filter = default;
        readonly EcsPoolInject<DisposeAllAbilityOnUnitEvent> _disposeAbilityPool = default;

        public override MainEcsSystem Clone()
        {
            return new DisposeAbilityAfterMomentDeadSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach(var entity in _filter.Value)
            {
                ref var disposeComp = ref _disposeAbilityPool.Value.Add(_world.Value.NewEntity());
                disposeComp.OwnerEntity = _world.Value.PackEntity(entity);
            }
        }
    }
}
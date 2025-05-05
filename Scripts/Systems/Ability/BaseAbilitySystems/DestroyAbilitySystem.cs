using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class DestroyAbilitySystem : MainEcsSystem
    {
        readonly EcsWorldInject _world = default;
        readonly EcsFilterInject<Inc<DestroyAbilityEvent>> _filter = default;
        readonly EcsPoolInject<DestroyAbilityEvent> _destroyAbilityPool = default;
        readonly EcsPoolInject<TimerAbilityComponent> _timerAbilityPool = default;

        public override MainEcsSystem Clone()
        {
            return new DestroyAbilitySystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach (var entity in _filter.Value)
            {
                ref var destroyComp = ref _destroyAbilityPool.Value.Get(entity);
                if(destroyComp.PackedEntity.Unpack(_world.Value, out int destroyedEntity))
                {
                    if(!_timerAbilityPool.Value.Has(destroyedEntity)) _world.Value.DelEntity(destroyedEntity);
                }
            }
        }
    }
}
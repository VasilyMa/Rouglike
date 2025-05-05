using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class HitDisposeAbilitySystem : MainEcsSystem { 
        readonly EcsWorldInject _world = default;
        readonly EcsFilterInject<Inc<TakeDamageComponent, HitAnimationAllowedComponent, CheckSideHitEvent>> _filter = default;
        readonly EcsPoolInject<TakeDamageComponent> _takeDamagePool = default;
        readonly EcsPoolInject<DisposeAllAbilityOnUnitEvent> _disposePool = default;
        public override MainEcsSystem Clone()
        {
            return new HitDisposeAbilitySystem();
        }
        public override void Run (IEcsSystems systems) {
            foreach(var entity in _filter.Value)
            {
                ref var takeDamageComp = ref _takeDamagePool.Value.Get(entity);
                if(takeDamageComp.TargetEntity.Unpack(_world.Value, out int targetEntity))
                {
                    ref var disposeComp = ref _disposePool.Value.Add(_world.Value.NewEntity());
                    disposeComp.OwnerEntity = takeDamageComp.TargetEntity;
                }
            }
        }
    }
}
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class CheckHealthAfterDamageSystem : MainEcsSystem {
        readonly EcsWorldInject _world = default;
        readonly EcsFilterInject<Inc<TakeDamageComponent, DamageAllowedComponent>> _filter = default;
        readonly EcsPoolInject<TakeDamageComponent> _takeDamagePool = default;
        readonly EcsPoolInject<HealthComponent> _healthPool = default;
        readonly EcsPoolInject<MomentDeadEvent> _momentDeadPool = default;
        public override MainEcsSystem Clone()
        {
            return new CheckHealthAfterDamageSystem();
        }
        public override void Run (IEcsSystems systems) {
            foreach(var entity in _filter.Value)
            {
                ref var takeDamageComp = ref _takeDamagePool.Value.Get(entity);
                if(takeDamageComp.TargetEntity.Unpack(_world.Value, out int targetEntity))
                {
                    ref var healthComp = ref _healthPool.Value.Get(targetEntity);

                    if (healthComp.GetCurrent() <= 0) 
                    {
                        if(!_momentDeadPool.Value.Has(targetEntity)) _momentDeadPool.Value.Add(targetEntity);
                        ref var momentDeadComp = ref _momentDeadPool.Value.Get(targetEntity);
                        momentDeadComp.killerEntity = takeDamageComp.KillerEntity;
                    }
                }
            }
        }
    }
}
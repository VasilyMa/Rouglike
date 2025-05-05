using Leopotam.EcsLite;
using UnityEngine;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class CheckDamageAllowedSystem : MainEcsSystem {
        readonly EcsWorldInject _world = default;
        readonly EcsFilterInject<Inc<TakeDamageComponent, DamageAllowedComponent>> _filter = default;
        readonly EcsPoolInject<TakeDamageComponent> _takeDamagePool = default;
        readonly EcsPoolInject<Invulnerable> _invulnerablePool = default;
        readonly EcsPoolInject<DamageAllowedComponent> _damageAllowedPool = default;
        readonly EcsPoolInject<HealthComponent> _healthPool = default;
        readonly EcsPoolInject<GlobalDamageCDComponent> _globalDamagePool;
        readonly EcsPoolInject<ConditionTakeDamageComponent> _conditionTakeDamagePool;
        public override MainEcsSystem Clone()
        {
            return new CheckDamageAllowedSystem();
        }
        public override void Run (IEcsSystems systems) {
            foreach(var entity in _filter.Value)
            {
                ref var takeDamageComp = ref _takeDamagePool.Value.Get(entity);
                if(takeDamageComp.TargetEntity.Unpack(_world.Value, out int targetEntity))
                {
                    if(_invulnerablePool.Value.Has(targetEntity) && !_conditionTakeDamagePool.Value.Has(entity)) _damageAllowedPool.Value.Del(entity);
                    if(!_healthPool.Value.Has(targetEntity)) _damageAllowedPool.Value.Del(entity);
                    if(_globalDamagePool.Value.Has(targetEntity) && !_conditionTakeDamagePool.Value.Has(entity)) _damageAllowedPool.Value.Del(entity);
                }
            }
        }
        
    }
}
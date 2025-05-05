using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class CheckToughnessDamageAllowedSystem : MainEcsSystem {
        readonly EcsWorldInject _world = default;
        readonly EcsFilterInject<Inc<TakeDamageComponent, ToughnessDamageAllowedComponent, DamageAllowedComponent>> _filter = default;
        readonly EcsFilterInject<Inc<TakeDamageComponent, ToughnessDamageAllowedComponent>, Exc<DamageAllowedComponent>> _filterExcDamage = default;
        readonly EcsPoolInject<ToughnessDamageAllowedComponent> _toughnessDamageAllowedPool = default;
        readonly EcsPoolInject<TakeDamageComponent> _takeDamagePool = default;
        readonly EcsPoolInject<ToughnessComponent> _toughnessPool = default;
        readonly EcsPoolInject<RecoveryToughnessComponent> _toughnessRecoveryPool = default;
        public override MainEcsSystem Clone()
        {
            return new CheckToughnessDamageAllowedSystem();
        }
        public override void Run (IEcsSystems systems) {
            foreach(var entity in _filter.Value)
            {
                ref var takeDamageComp = ref _takeDamagePool.Value.Get(entity);
                if(takeDamageComp.TargetEntity.Unpack(_world.Value, out int targetEntity))
                {
                    if(takeDamageComp.Damage <= 0) _toughnessDamageAllowedPool.Value.Del(entity);
                    if(!_toughnessPool.Value.Has(targetEntity)) _toughnessDamageAllowedPool.Value.Del(entity);
                    if (_toughnessRecoveryPool.Value.Has(targetEntity)) _toughnessDamageAllowedPool.Value.Del(entity);
                }
            }
            foreach(var entity in _filterExcDamage.Value)
            {
                _toughnessDamageAllowedPool.Value.Del(entity);
            }
        }
    }
}
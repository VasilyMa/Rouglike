using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class CheckNumberDamageAllowedSystem : MainEcsSystem {
        readonly EcsWorldInject _world = default;
        readonly EcsFilterInject<Inc<TakeDamageComponent, NumberDamageAllowedComponent>> _filter = default;
        readonly EcsPoolInject<TakeDamageComponent> _takeDamagePool = default;
        readonly EcsPoolInject<NumberDamageAllowedComponent> _numberDamageAllowedPool = default;
        readonly EcsPoolInject<DamageAllowedComponent> _damageAllowedPool = default;
        public override MainEcsSystem Clone()
        {
            return new CheckNumberDamageAllowedSystem();
        }
        public override void Run (IEcsSystems systems) {
            foreach(var entity in _filter.Value)
            {
                if(!_damageAllowedPool.Value.Has(entity)) _numberDamageAllowedPool.Value.Del(entity);
            }
        }
    }
}
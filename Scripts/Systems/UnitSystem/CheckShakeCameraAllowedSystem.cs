using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class CheckShakeCameraAllowedSystem : MainEcsSystem {
        readonly EcsFilterInject<Inc<TakeDamageComponent, ShakeCameraAllowedComponent, DamageAllowedComponent>> _filter = default;
        readonly EcsFilterInject<Inc<TakeDamageComponent, ShakeCameraAllowedComponent>, Exc<DamageAllowedComponent>> _filterExcDamage = default;
        readonly EcsPoolInject<TakeDamageComponent> _takeDamagePool = default;
        readonly EcsPoolInject<ShakeCameraAllowedComponent> _shakeCameraAllowedPool = default;
        public override MainEcsSystem Clone()
        {
            return new CheckShakeCameraAllowedSystem();
        }
        public override void Run (IEcsSystems systems) {
            foreach(var entity in _filter.Value)
            {
                //todo логика снятия разрешения
            }
            foreach(var entity in _filterExcDamage.Value)
            {
                _shakeCameraAllowedPool.Value.Del(entity);
            }
        }
    }
}
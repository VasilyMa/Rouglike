using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class ClearAllAllowedComponentsOnTakeDamageEntitySystem : MainEcsSystem {
        readonly EcsFilterInject<Inc<TakeDamageComponent, ClearAllAllowedComponents>> _filter = default;
        readonly EcsPoolInject<VisualAllowedComponent> _visualAllowedPool = default;
        readonly EcsPoolInject<DamageAllowedComponent> _damageAllowedPool = default;
        readonly EcsPoolInject<NumberDamageAllowedComponent> _numberDamageAllowedPool = default;
        readonly EcsPoolInject<SoundDamageAllowedComponent> _soundAllowedPool = default;
        readonly EcsPoolInject<HitAnimationAllowedComponent> _hitAnimationAllowedPool = default;
        readonly EcsPoolInject<ShakeCameraAllowedComponent> _shakeCameraAllowedPool = default;
        readonly EcsPoolInject<ToughnessDamageAllowedComponent> _toughnessDamageAllowedPool = default;
        readonly EcsPoolInject<ShieldDamageAllowedComponent> _shieldDamageAllowedPool = default;

        public override MainEcsSystem Clone()
        {
            return new ClearAllAllowedComponentsOnTakeDamageEntitySystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach(var entity in _filter.Value)
            {
                _visualAllowedPool.Value.Del(entity);
                _damageAllowedPool.Value.Del(entity);
                _numberDamageAllowedPool.Value.Del(entity);
                _soundAllowedPool.Value.Del(entity);
                _hitAnimationAllowedPool.Value.Del(entity);
                _shakeCameraAllowedPool.Value.Del(entity);
                _toughnessDamageAllowedPool.Value.Del(entity);
                _shieldDamageAllowedPool.Value.Del(entity);
            }
        }
    }
}
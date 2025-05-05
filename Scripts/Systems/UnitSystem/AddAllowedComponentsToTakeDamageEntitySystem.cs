using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client {
    sealed class AddAllowedComponentsToTakeDamageEntitySystem : MainEcsSystem {
        readonly EcsFilterInject<Inc<TakeDamageComponent>> _filter = default;
        readonly EcsPoolInject<DamageAllowedComponent> _damageAllowedPool = default;
        readonly EcsPoolInject<VisualAllowedComponent> _visualAllowedPool = default;
        readonly EcsPoolInject<NumberDamageAllowedComponent> _numberAllowedPool = default;
        readonly EcsPoolInject<SoundDamageAllowedComponent> _soundAllowedPool = default;
        readonly EcsPoolInject<HitAnimationAllowedComponent> _hitAnimationAllowedPool = default;
        readonly EcsPoolInject<ShakeCameraAllowedComponent> _shakeCameraAllowedPool = default;
        readonly EcsPoolInject<ToughnessDamageAllowedComponent> _toughnessDamageAllowedPool = default;
        readonly EcsPoolInject<ShieldDamageAllowedComponent> _shieldDamageAllowedPool = default;
        public override MainEcsSystem Clone()
        {
            return new AddAllowedComponentsToTakeDamageEntitySystem();
        }
        public override void Run (IEcsSystems systems) {
            foreach(var entity in _filter.Value)
            {
                _damageAllowedPool.Value.Add(entity);
                _visualAllowedPool.Value.Add(entity);
                _numberAllowedPool.Value.Add(entity);
                _soundAllowedPool.Value.Add(entity);
                _hitAnimationAllowedPool.Value.Add(entity);
                _shakeCameraAllowedPool.Value.Add(entity);
                _toughnessDamageAllowedPool.Value.Add(entity);
                _shieldDamageAllowedPool.Value.Add(entity);
            }
        }
    }
}
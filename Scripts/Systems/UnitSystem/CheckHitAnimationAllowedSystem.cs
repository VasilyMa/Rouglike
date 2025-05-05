using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client {
    sealed class CheckHitAnimationAllowedSystem : MainEcsSystem {    
        readonly EcsWorldInject _world = default;
        readonly EcsFilterInject<Inc<TakeDamageComponent, HitAnimationAllowedComponent>> _filter = default;
        readonly EcsPoolInject<TakeDamageComponent> _takeDamagePool = default;
        readonly EcsPoolInject<HitAnimationAllowedComponent> _hitAllowedPool = default;
        readonly EcsPoolInject<HighToughnessComponent> _highToughnessPool = default;
        readonly EcsPoolInject<IrrevocabilityComponent> _irrevocabilityPool = default;
        readonly EcsPoolInject<KnockbackEffect> _knockBackPool = default;
        readonly EcsPoolInject<HitAnimationState> _hitAnimationStatePool = default;
        readonly EcsPoolInject<HardHitComponent> _hardHitPool = default;
        readonly EcsPoolInject<ConditionTakeDamageComponent> _conditionTakeDamagePool = default;
        readonly EcsPoolInject<PlayerComponent> _playerPool = default;
        readonly EcsPoolInject<InActionComponent> _inActionPool = default;
        public override MainEcsSystem Clone()
        {
            return new CheckHitAnimationAllowedSystem();
        }
        public override void Run (IEcsSystems systems) {
            foreach(var entity in _filter.Value)
            {
                ref var takeDamageComp = ref _takeDamagePool.Value.Get(entity);
                if(takeDamageComp.TargetEntity.Unpack(_world.Value, out int targetEntity))
                {
                    if(_highToughnessPool.Value.Has(targetEntity)) _hitAllowedPool.Value.Del(entity);
                    if(_irrevocabilityPool.Value.Has(targetEntity)) _hitAllowedPool.Value.Del(entity);
                    if(_knockBackPool.Value.Has(targetEntity)) _hitAllowedPool.Value.Del(entity);
                    if(_hitAnimationStatePool.Value.Has(targetEntity)) _hitAllowedPool.Value.Del(entity);
                    if(_hardHitPool.Value.Has(targetEntity)) _hitAllowedPool.Value.Del(entity);
                    if (_playerPool.Value.Has(targetEntity) && _inActionPool.Value.Has(targetEntity)) _hitAllowedPool.Value.Del(entity);
                }

                if(_conditionTakeDamagePool.Value.Has(entity)) _hitAllowedPool.Value.Del(entity);
            }
        }
    }
}
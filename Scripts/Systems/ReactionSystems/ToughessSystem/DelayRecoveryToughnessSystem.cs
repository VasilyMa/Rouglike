using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client {
    sealed class DelayRecoveryToughnessSystem : MainEcsSystem
    {
        readonly EcsFilterInject<Inc<ToughnessComponent, DelayRecoveryToughnessComponent>, Exc<DeadComponent>> _filterDelay;
        readonly EcsPoolInject<ToughnessComponent> _toughnessPool;
        readonly EcsPoolInject<DelayRecoveryToughnessComponent> _delayRecovereToughnessPool;
        readonly EcsPoolInject<RemoveDelayRecoveryToughnessEvent> _removeDelayRecoveryEvent;

        public override MainEcsSystem Clone()
        {
            return new DelayRecoveryToughnessSystem();
        }

        public override void Run (IEcsSystems systems) 
        {
            foreach(var entity in _filterDelay.Value)
            {
                ref var delayRecovery = ref _delayRecovereToughnessPool.Value.Get(entity);
                delayRecovery.DelayRecovery -= Time.deltaTime;
                if (delayRecovery.DelayRecovery > 0) continue;
                _removeDelayRecoveryEvent.Value.Add(entity);
            }
        }
    }
}
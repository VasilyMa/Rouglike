using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class RemoveDelayRecoveryToughnessSystem: MainEcsSystem 
    {
        readonly EcsFilterInject<Inc<RemoveDelayRecoveryToughnessEvent>, Exc<RecoveryToughnessComponent>> _filterRemove;
        readonly EcsPoolInject<DelayRecoveryToughnessComponent> _delayRecovereToughnessPool;
        readonly EcsPoolInject<RecoveryToughnessComponent> _recoveryToughnessPool;

        public override MainEcsSystem Clone()
        {
            return new RemoveDelayRecoveryToughnessSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach ( var entity in _filterRemove.Value)
            {
                _delayRecovereToughnessPool.Value.Del(entity);
                _recoveryToughnessPool.Value.Add(entity);
            }
        }
    }
}
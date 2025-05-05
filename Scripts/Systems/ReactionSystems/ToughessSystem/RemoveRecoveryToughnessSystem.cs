using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class RemoveRecoveryToughnessSystem : MainEcsSystem
    {
        readonly EcsFilterInject<Inc<RemoveRecoveryToughnessEvent>> _filterRemove;
        readonly EcsPoolInject<RecoveryToughnessComponent> _recoveryToughnessPool;
        readonly EcsPoolInject<HighToughnessComponent> _highToughnessPool = default;

        public override MainEcsSystem Clone()
        {
            return new RemoveRecoveryToughnessSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach(var entity in _filterRemove.Value)
            {
                _recoveryToughnessPool.Value.Del(entity);
                _highToughnessPool.Value.Add(entity);
            }
        }
    }
}
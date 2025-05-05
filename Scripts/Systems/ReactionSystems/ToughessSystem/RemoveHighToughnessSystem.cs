using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class RemoveHighToughnessSystem : MainEcsSystem
    {
        readonly EcsFilterInject<Inc<RemoveHighToughnessEvent,ToughnessComponent>, Exc<DelayRecoveryToughnessComponent>> _filterRemove;
        readonly EcsPoolInject<HighToughnessComponent> _highToughnessPool = default;
        readonly EcsPoolInject<DelayRecoveryToughnessComponent> _delayRecoveryPool;
        readonly EcsPoolInject<ToughnessComponent> _toughnessPool = default;

        public override MainEcsSystem Clone()
        {
            return new RemoveHighToughnessSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach(var entity in _filterRemove.Value)
            {
                _highToughnessPool.Value.Del(entity);
                ref var toughnessComp = ref _toughnessPool.Value.Get(entity);
                if (!_delayRecoveryPool.Value.Has(entity)) _delayRecoveryPool.Value.Add(entity);
                ref var delayComp = ref _delayRecoveryPool.Value.Get(entity);
                delayComp.DelayRecovery = toughnessComp.DelayRecoveryToughness;
            }
        }
    }
}
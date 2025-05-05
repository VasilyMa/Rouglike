using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client {
    sealed class RecoveryToughnessSystem : MainEcsSystem
    {
        readonly EcsFilterInject<Inc<ToughnessComponent, RecoveryToughnessComponent>, Exc<DeadComponent>> _filterRecovery;
        readonly EcsPoolInject<ToughnessComponent> _toughnessPool;
        readonly EcsPoolInject<RemoveRecoveryToughnessEvent> _removeRecoveryToughnessEvent;

        public override MainEcsSystem Clone()
        {
            return new RecoveryToughnessSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach (var entity in _filterRecovery.Value)
            {
                ref var toughnessComp = ref _toughnessPool.Value.Get(entity);
                toughnessComp.CurrentValue += toughnessComp.SpeedRecovery * Time.deltaTime;
                if (toughnessComp.CurrentValue < toughnessComp.MaxValueToughness) continue;
                toughnessComp.CurrentValue = toughnessComp.MaxValueToughness;
                _removeRecoveryToughnessEvent.Value.Add(entity);
            }
        }
    }
}
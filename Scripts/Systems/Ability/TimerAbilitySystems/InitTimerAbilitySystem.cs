using Leopotam.EcsLite;
using UnityEngine;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class InitTimerAbilitySystem : MainEcsSystem
    {
        readonly EcsFilterInject<Inc<InitTimerAbilityEvent, AbilityComponent>, Exc<TimerAbilityComponent>> _filter = default;
        readonly EcsPoolInject<TimerAbilityComponent> _timerPool = default;
        readonly EcsPoolInject<TimeLineBlocksListAbilityComponent> _blockPool = default;

        public override MainEcsSystem Clone()
        {
            return new InitTimerAbilitySystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach (var entity in _filter.Value)
            {
                ref var timerComp = ref _timerPool.Value.Add(entity);
                ref var blockPool = ref _blockPool.Value.Get(entity);
                timerComp.BlocksList = new System.Collections.Generic.List<AbilitySystem.TimeLineBlock>(blockPool.BlockList);
            }
        }
    }
}
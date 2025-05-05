using Leopotam.EcsLite;
using UnityEngine;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class TimerAbilitySystem : MainEcsSystem
    {
        readonly EcsFilterInject<Inc<TimerAbilityComponent>> _filter = default;
        readonly EcsPoolInject<TimerAbilityComponent> _timerAbilityPool = default;

        public override MainEcsSystem Clone()
        {
            return new TimerAbilitySystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach(var entity in _filter.Value)
            {
                ref var timerComp = ref _timerAbilityPool.Value.Get(entity);
                timerComp.Timer += Time.deltaTime;
            }
        }
    }
}
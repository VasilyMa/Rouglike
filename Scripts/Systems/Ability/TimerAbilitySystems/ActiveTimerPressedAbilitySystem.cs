using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class ActiveTimerPressedAbilitySystem : MainEcsSystem
    {
        readonly EcsFilterInject<Inc<TimerStartAtPressedComponent, AbilityPressedEvent, CheckAbilityToUse>, Exc<TimerAbilityComponent>> _filter = default;
        readonly EcsPoolInject<InitTimerAbilityEvent> _initTimerPool = default;

        public override MainEcsSystem Clone()
        {
            return new ActiveTimerPressedAbilitySystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach(var entity in _filter.Value)
            {
                _initTimerPool.Value.Add(entity);
            }
        }
    }
}
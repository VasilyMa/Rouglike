using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
namespace Client {
    sealed class ActiveTimerReleasedAbilitySystem : MainEcsSystem 
    { 
        readonly EcsFilterInject<Inc<TimerStartAtReleasedComponent, AbilityReleasedEvent, CheckAbilityToUse>, Exc<TimerAbilityComponent>> _filter = default;
        readonly EcsPoolInject<InitTimerAbilityEvent> _initTimerPool = default;

        public override MainEcsSystem Clone()
        {
            return new ActiveTimerReleasedAbilitySystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach(var entity in _filter.Value)
            {
                _initTimerPool.Value.Add(entity);
            }
        }
    }
}
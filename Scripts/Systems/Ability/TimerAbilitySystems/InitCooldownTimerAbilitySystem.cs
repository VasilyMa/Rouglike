using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class InitCooldownTimerAbilitySystem : MainEcsSystem 
    {
        readonly EcsFilterInject<Inc<InitTimerAbilityEvent, AbilityComponent,CoolDownComponent>,Exc<CooldownRecalculationComponent, PlayerAbilityComponent>> _filter = default;
        readonly EcsPoolInject<CooldownRecalculationComponent> _coolDownRecalculationPool = default;
        readonly EcsPoolInject<CoolDownComponent> _coolDownPool = default;

        public override MainEcsSystem Clone()
        {
            return new InitCooldownTimerAbilitySystem();
        }

        public override void Run (IEcsSystems systems) {
           foreach(var entity in _filter.Value)
            {
                _coolDownRecalculationPool.Value.Add(entity);
                ref var coolDownComp = ref _coolDownPool.Value.Get(entity);
                coolDownComp.CurrentCoolDownValue = coolDownComp.CoolDownValue;
            }
        }
    }
}
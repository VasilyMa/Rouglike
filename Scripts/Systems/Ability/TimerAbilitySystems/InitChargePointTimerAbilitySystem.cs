using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class InitChargePointTimerAbilitySystem : MainEcsSystem
    {
        readonly EcsFilterInject<Inc<InitTimerAbilityEvent, ChargePointComponent>> _filter = default;
        readonly EcsPoolInject<ChargePointComponent> _chargePointPool = default;

        public override MainEcsSystem Clone()
        {
            return new InitChargePointTimerAbilitySystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach(var entity in _filter.Value)
            {
                ref var chargePointComp = ref _chargePointPool.Value.Get(entity);
                chargePointComp.CurrentChargeCount--;
                chargePointComp.OnChargePointChange?.Invoke(chargePointComp.CurrentChargeCount);
            }
        }
    }
}
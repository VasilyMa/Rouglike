using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
namespace Client {
    sealed class CheckCooldownTimerWithChargePointAbilitySystem : MainEcsSystem
    {
        readonly EcsFilterInject<Inc<CoolDownComponent, CooldownRecalculationComponent, ChargePointComponent>> _filter = default;
        readonly EcsPoolInject<CoolDownComponent> _coolDownPool = default;
        readonly EcsPoolInject<ChargePointComponent> _chargePointPool = default;
        readonly EcsPoolInject<CooldownRecalculationComponent> _recalculationPool = default;

        public override MainEcsSystem Clone()
        {
            return new CheckCooldownTimerWithChargePointAbilitySystem();
        }

        public override void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter.Value)
            {
                ref var coolDownComp = ref _coolDownPool.Value.Get(entity);
                ref var chargePointComp = ref _chargePointPool.Value.Get(entity);

                if (coolDownComp.CurrentCoolDownValue <= 0)
                {
                    coolDownComp.CurrentCoolDownValue = coolDownComp.CoolDownValue;
                    chargePointComp.CurrentChargeCount++;
                    chargePointComp.CurrentChargeCount = Mathf.Clamp(chargePointComp.CurrentChargeCount, 0, chargePointComp.MaxChargeCount);
                    chargePointComp.OnChargePointChange?.Invoke(chargePointComp.CurrentChargeCount);
                    if (chargePointComp.CurrentChargeCount >= chargePointComp.MaxChargeCount)
                    {
                        _recalculationPool.Value.Del(entity);
                    }
                }
            }
        }
    }
}
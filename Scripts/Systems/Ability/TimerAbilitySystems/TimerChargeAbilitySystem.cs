using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client {
    sealed class TimerChargeAbilitySystem : MainEcsSystem 
    {
        readonly EcsFilterInject<Inc<ChargeComponent, ChargeRecalculateComponent>> _filter = default;
        readonly EcsPoolInject<ChargeComponent> _chargePool = default;
        readonly EcsPoolInject<FullChargeAbilityEvent> _fullChargeAbilityPool = default;

        public override MainEcsSystem Clone()
        {
            return new TimerChargeAbilitySystem();
        }

        public override void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter.Value)
            {
                ref var chargeComp = ref _chargePool.Value.Get(entity);
                chargeComp.CurrentChargeTimer += Time.deltaTime;
                chargeComp.CurrentCharge = chargeComp.CurrentChargeTimer / chargeComp.ChargeMaxTime;
                if(chargeComp.CurrentCharge >= 1)
                {
                    _fullChargeAbilityPool.Value.Add(entity);
                }
            }
        }
    }
}
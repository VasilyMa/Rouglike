using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using Statement;

using UnityEngine;


namespace Client 
{
    sealed class AbilityObserverSystem : MainEcsSystem 
    {
        readonly EcsFilterInject<Inc<AbilityComponent, AbilityObserverComponent, CoolDownComponent>> _filter = default;
        readonly EcsFilterInject<Inc<AbilityComponent, AbilityObserverComponent, ChargePointComponent>> _chargeFilter = default;
        readonly EcsPoolInject<CoolDownComponent> _cooldownPool = default;
        readonly EcsPoolInject<ChargePointComponent> _chargePool = default;
        readonly EcsPoolInject<AbilityObserverComponent> _abilityObserverPool = default;

        public override MainEcsSystem Clone()
        {
            return new AbilityObserverSystem();
        }

        public override void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter.Value)
            {
                ref var abilityObserverComp = ref _abilityObserverPool.Value.Get(entity);
                ref var cooldownComp = ref _cooldownPool.Value.Get(entity);

                abilityObserverComp.CooldownValue.Value = new CooldownValue(cooldownComp.CurrentCoolDownValue, cooldownComp.CoolDownValue);
            }
            foreach (var chargeEntity in _chargeFilter.Value)
            {
                ref var abilityObserverComp = ref _abilityObserverPool.Value.Get(chargeEntity);
                ref var chargeComp = ref _chargePool.Value.Get(chargeEntity);

                abilityObserverComp.ChargeValue.Value = new ChargeValue(chargeComp.CurrentChargeCount, chargeComp.MaxChargeCount);
            }

        }
    }
}
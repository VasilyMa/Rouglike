using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class ActiveChargeTimerSystem : MainEcsSystem
    {
        readonly EcsFilterInject<Inc<AbilityPressedEvent, ChargeComponent, CheckAbilityToUse>> _filter = default;
        readonly EcsFilterInject<Inc<AbilityReleasedEvent, ChargeComponent, CheckAbilityToUse>> _releasedFilter = default;
        readonly EcsPoolInject<ChargeRecalculateComponent> _recalculatePool = default;
        readonly EcsPoolInject<ChargeComponent> _chargePool = default;
        readonly EcsPoolInject<EndChargeAbilityEvent> _endChargeComp = default;

        public override MainEcsSystem Clone()
        {
            return new ActiveChargeTimerSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach (var entity in _filter.Value)
            { 
                if(!_recalculatePool.Value.Has(entity)) _recalculatePool.Value.Add(entity);
                ref var chargeComp = ref _chargePool.Value.Get(entity);
                chargeComp.CurrentChargeTimer = 0;
            }
            foreach(var entity in _releasedFilter.Value)
            {
                _endChargeComp.Value.Add(entity);
            }
        }
    }
}
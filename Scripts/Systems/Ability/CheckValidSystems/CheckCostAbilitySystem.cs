using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class CheckCostAbilitySystem : MainEcsSystem 
    {
        readonly EcsFilterInject<Inc<ChargePointComponent, CheckAbilityToUse>, Exc<DeleteCheckAbilityToUseEvent>> _filter = default;
        readonly EcsPoolInject<ChargePointComponent> _chargePointPool = default;
        readonly EcsPoolInject<DeleteCheckAbilityToUseEvent> _deleteCheckPool = default;

        public override MainEcsSystem Clone()
        {
            return new CheckCostAbilitySystem();
        }

        public override void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter.Value)
            {
                ref var chargePointComp = ref _chargePointPool.Value.Get(entity);
                if(chargePointComp.CurrentChargeCount <= 0)
                {
                    _deleteCheckPool.Value.Add(entity);
                }
            }
        }
    }
}
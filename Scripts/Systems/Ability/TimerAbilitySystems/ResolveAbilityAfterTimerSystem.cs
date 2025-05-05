using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class ResolveAbilityAfterTimerSystem : MainEcsSystem {
        readonly EcsWorldInject _world = default;
        readonly EcsFilterInject<Inc<ResolveAbilityAfterTimerEvent, TimerAbilityComponent>> _filter = default;
        readonly EcsPoolInject<TimerAbilityComponent> _timerAbilityPool = default;
        readonly EcsPoolInject<OwnerComponent> _ownerPool = default;
        readonly EcsPoolInject<ChargeComponent> _chargePool = default;

        public override MainEcsSystem Clone()
        {
            return new ResolveAbilityAfterTimerSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach (var entity in _filter.Value)
            {
                ref var timerComp = ref _timerAbilityPool.Value.Get(entity);
                ref var ownerComp = ref _ownerPool.Value.Get(entity);
                if (ownerComp.OwnerEntity.Unpack(_world.Value, out int ownerEntity))
                {
                    float chargeValue = 1f;
                    if(_chargePool.Value.Has(entity))
                    {
                        ref var chargeComp = ref _chargePool.Value.Get(entity);
                        chargeValue = chargeComp.CurrentCharge;
                    }
                    foreach(var comp in timerComp.BlocksList[0].FXComponents)
                    {
                        comp.Invoke(ownerEntity, entity, _world.Value, chargeValue);
                    }
                }
                
                timerComp.BlocksList.Remove(timerComp.BlocksList[0]);
            }
        }
    }
}
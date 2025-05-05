using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class FullChargeAbilitySystem : MainEcsSystem 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsFilterInject<Inc<FullChargeAbilityEvent, ChargeComponent, AbilityComponent>> _filter = default;
        readonly EcsPoolInject<OwnerComponent> _ownerPool = default;
        readonly EcsPoolInject<ChargeComponent> _chargePool = default;

        public override MainEcsSystem Clone()
        {
            return new FullChargeAbilitySystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach (var entity in _filter.Value)
            {
                ref var ownerComp = ref _ownerPool.Value.Get(entity);
                if(ownerComp.OwnerEntity.Unpack(_world.Value, out int ownerEntity))
                {
                    ref var chargeComp = ref _chargePool.Value.Get(entity);
                    foreach(var comp in chargeComp.FullChargeComponents)
                    {
                        comp.Invoke(ownerEntity, _world.Value);
                    }
                }
            }
        }
    }
}
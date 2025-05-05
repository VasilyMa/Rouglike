using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;


namespace Client {
    sealed class RecalculateResolveBlockSystem : MainEcsSystem 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsFilterInject<Inc<RecalculateResolveBlockEvent>> _filter = default;
        readonly EcsPoolInject<RecalculateResolveBlockEvent> _eventPool = default;
        readonly EcsPoolInject<ResolveBlocksAbilityComponent> _resolvePool = default;
        readonly EcsPoolInject<ResolveBlockComponent> _resolveBlockPool = default;
        readonly EcsPoolInject<ChargeComponent> _chargePool = default;

        public override MainEcsSystem Clone()
        {
            return new RecalculateResolveBlockSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach (var entity in _filter.Value)
            {
                ref var evtComp = ref _eventPool.Value.Get(entity);
                if(evtComp.AbilityEntity.Unpack(_world.Value, out int abilityEntity))
                {
                    if(_resolvePool.Value.Has(abilityEntity))
                    {
                        ref var resolveAbilityComp = ref _resolvePool.Value.Get(abilityEntity);
                        ref var resolveBlockComp = ref _resolveBlockPool.Value.Add(entity);
                        resolveBlockComp.Components = new System.Collections.Generic.List<AbilitySystem.IAbilityEffect>(resolveAbilityComp.Components);
                        float chargeValue = 1;
                        if(_chargePool.Value.Has(abilityEntity))
                        {
                            ref var chargeComp = ref _chargePool.Value.Get(abilityEntity);
                            chargeValue = chargeComp.CurrentCharge;
                        }
                        
                        foreach (var comp in resolveBlockComp.Components)
                        {
                            comp.Recalculate(chargeValue);
                        }
                    }
                }
            }
        }
    }
}
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class UseAbilitySystem : MainEcsSystem {
        readonly EcsWorldInject _world = default;
        readonly EcsFilterInject<Inc<AbilityComponent, CheckAbilityToUse, AbilityPressedEvent>, Exc<TimerAbilityComponent>> _pressedFilter = default;
        readonly EcsFilterInject<Inc<AbilityComponent, CheckAbilityToUse, AbilityReleasedEvent>, Exc<TimerAbilityComponent>> _releasedFilter = default; 
        readonly EcsPoolInject<AbilityComponent> _abilityPool = default;
        readonly EcsPoolInject<OwnerComponent> _ownerPool = default;

        public override MainEcsSystem Clone()
        {
            return new UseAbilitySystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach(var pressedAbility in _pressedFilter.Value)
            {
                ref var abilityComp = ref _abilityPool.Value.Get(pressedAbility);
                ref var ownerComp = ref _ownerPool.Value.Get(pressedAbility);
                if (ownerComp.OwnerEntity.Unpack(_world.Value, out int ownerEntity))
                {
                    foreach(var block in abilityComp.Ability.SourceAbility.InputBlocks)
                    {
                        if(!block.Pressing) continue;
                        foreach(var comp in block.Components)
                        {
                            comp.Invoke(ownerEntity, pressedAbility, _world.Value);
                        }
                    }
                }
                
            }
            foreach(var releasedAbility in _releasedFilter.Value)
            {
                ref var abilityComp = ref _abilityPool.Value.Get(releasedAbility);
                ref var ownerComp = ref _ownerPool.Value.Get(releasedAbility);
                if (ownerComp.OwnerEntity.Unpack(_world.Value, out int ownerEntity))
                {
                    foreach(var block in abilityComp.Ability.SourceAbility.InputBlocks)
                    {
                        if(block.Pressing) continue;
                        foreach(var comp in block.Components)
                        {
                            comp.Invoke(ownerEntity, releasedAbility, _world.Value);
                        }
                    }
                }
            }
        }
    }
}
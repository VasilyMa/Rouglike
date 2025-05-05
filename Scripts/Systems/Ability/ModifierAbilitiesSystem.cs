using UnityEngine;
using System;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using System.Collections.Generic;


namespace Client {
    sealed class ModifierAbilitiesSystem : MainEcsSystem {   
        readonly EcsWorldInject _world = default;
        readonly EcsFilterInject<Inc<PlayerComponent, ModifiersContainer, ModifiersContainerChangesEvent>> _filter = default;
        readonly EcsFilterInject<Inc<PlayerAbilityComponent, AbilityComponent>> _abilityFilter = default;
        readonly EcsPoolInject<AbilityComponent> _abilityPool = default;
        readonly EcsPoolInject<ModifiersContainer> _modPool = default;
        readonly EcsPoolInject<ResolveBlocksAbilityComponent> _resolveBlockPool = default;
        public override MainEcsSystem Clone()
        {
            return new ModifierAbilitiesSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach(var entity in _filter.Value)
            {
                ref var modComp = ref _modPool.Value.Get(entity);

                foreach(var abilityEntity in _abilityFilter.Value)
                {
                    ref var abilityComp = ref _abilityPool.Value.Get(abilityEntity);
                    foreach(var modifier in modComp.Modifiers)
                    {
                        if((modifier.ModifierTags & abilityComp.Ability.ModifierTags) == modifier.ModifierTags)
                        {
                            int requestModifierEntity = _world.Value.NewEntity();
                            if(modifier.Target != null) modifier.Target.Init(abilityEntity, requestModifierEntity, _world.Value, modifier);
                            if(modifier.ModifierType != null) modifier.ModifierType.Init(requestModifierEntity, _world.Value);
                        }
                    }
                }
            }
        }
    }
}
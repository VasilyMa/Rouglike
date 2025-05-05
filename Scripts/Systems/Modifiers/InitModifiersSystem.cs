using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client {
    sealed class InitModifiersSystem : MainEcsSystem {
        readonly EcsWorldInject _world = default;
        readonly EcsFilterInject<Inc<InitModifiersEvent, PlayerComponent, AbilityUnitComponent>, Exc<ModifiersContainer>> _filter = default;
        readonly EcsPoolInject<AbilityUnitComponent> _abilityUnitPool = default;
        readonly EcsPoolInject<ModifiersContainer> _playerModifierPool = default;
        readonly EcsPoolInject<RequestAddModifier> _addModifierPool = default;

        public override MainEcsSystem Clone()
        {
            return new InitModifiersSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach(var entity in _filter.Value)
            {
                ref var abilityUnitComp = ref _abilityUnitPool.Value.Get(entity);
                ref var playerModComp = ref _playerModifierPool.Value.Add(entity);
                playerModComp.Modifiers = new System.Collections.Generic.List<Modifier>();
                foreach(var mod in abilityUnitComp.AbilityUnitMB.TEST_MODIFIERS_DELETE_AFTER_TEST)
                {
                    
                    ref var addModifierComp = ref _addModifierPool.Value.Add(_world.Value.NewEntity());
                    addModifierComp.UnitPackedEntity = _world.Value.PackEntity(entity);
                    addModifierComp.Modifier = mod;
                }
            }
        }
    }
}
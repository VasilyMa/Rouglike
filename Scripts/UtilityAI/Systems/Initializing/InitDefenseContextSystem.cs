using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using System.Collections.Generic;

namespace Client {
    sealed class InitDefenseContextSystem : MainEcsSystem {
        /// <summary>
        /// System catches InitAIEvent and initializes all Entities with UnitBrain and DefenseContext components
        /// </summary>
        readonly private EcsFilterInject<Inc<UnitBrain, DefenseContext, InitContextEvent>> _filter = default;
        readonly private EcsPoolInject<DefenseContext> _defenseContextPool = default;
        readonly private EcsPoolInject<AbilityUnitComponent> _abilityUnitPool = default;
        readonly private EcsPoolInject<AbilityComponent> _abilityPool = default;
        readonly private EcsWorldInject _world = default;

        public override MainEcsSystem Clone()
        {
            return new InitDefenseContextSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach (int unitEntity in _filter.Value)
            {
                ref var defenseContext = ref _defenseContextPool.Value.Get(unitEntity);
                ref var abilityUnitComp = ref _abilityUnitPool.Value.Get(unitEntity);

                defenseContext.defenseActionsList = new List<EcsPackedEntity>();

                foreach (var abilityEntity in abilityUnitComp.AbilityUnitMB.GetAllAbilitiesEntities())
                {
                    ref var abilityComp = ref _abilityPool.Value.Get(abilityEntity);
                    if (abilityComp.Ability.SourceAbility.AbilityType == AbilitySystem.AbilityTypes.Defence)
                        defenseContext.defenseActionsList.Add(_world.Value.PackEntity(abilityEntity));
                }
            }
        }
    }
}
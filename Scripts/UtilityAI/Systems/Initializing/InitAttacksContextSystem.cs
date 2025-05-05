using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using System.Collections.Generic;
namespace Client {
    /// <summary>
    /// Reserve system for Attacks Context initializing. 
    /// Iterates through all entities with UnitBrain and AttacksContext
    /// </summary>
    sealed class InitAttacksContextSystem : MainEcsSystem {
        readonly EcsWorldInject _world = default;
        readonly EcsFilterInject<Inc<UnitBrain, AttacksContext, InitContextEvent>> _filter = default;
        readonly EcsPoolInject<AttacksContext> _attacksContextPool = default;
        readonly EcsPoolInject<AbilityUnitComponent> _abilityUnitPool = default;
        readonly EcsPoolInject<AbilityComponent> _abilityPool = default;

        public override MainEcsSystem Clone()
        {
            return new InitAttacksContextSystem();
        }

        public override void Run (IEcsSystems systems) 
        {
            foreach (int unitEntity in _filter.Value)
            {
                ref var attacksContext = ref _attacksContextPool.Value.Get(unitEntity);
                ref var abilityUnitComp = ref _abilityUnitPool.Value.Get(unitEntity);
                attacksContext.attackAbilitiesList = new List<EcsPackedEntity>();
                attacksContext.validAbilitiesList = new List<EcsPackedEntity>();

                foreach (var abilityEntity in abilityUnitComp.AbilityUnitMB.GetAllAbilitiesEntities())
                {
                    ref var abilityComp = ref _abilityPool.Value.Get(abilityEntity);
                    if (abilityComp.Ability.SourceAbility.AbilityType == AbilitySystem.AbilityTypes.Attack)
                        attacksContext.attackAbilitiesList.Add(_world.Value.PackEntity(abilityEntity));
                }
            }
        }
    }
}
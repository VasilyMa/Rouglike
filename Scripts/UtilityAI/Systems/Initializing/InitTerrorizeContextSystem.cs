using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using System.Collections.Generic;

namespace Client {
    sealed class InitTerrorizeContextSystem : MainEcsSystem
    {
        readonly EcsWorldInject _world = default;
        readonly EcsFilterInject<Inc<UnitBrain, TerrorizeContext, InitContextEvent>> _filter = default;
        readonly EcsPoolInject<TerrorizeContext> _terrorizeContextPool = default;
        readonly EcsPoolInject<AbilityUnitComponent> _abilityUnitPool = default;
        readonly EcsPoolInject<AbilityComponent> _abilityPool = default;

        public override MainEcsSystem Clone()
        {
            return new InitTerrorizeContextSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach (int unitEntity in _filter.Value)
            {
                ref var terrorizeContext = ref _terrorizeContextPool.Value.Get(unitEntity);
                ref var abilityUnitComp = ref _abilityUnitPool.Value.Get(unitEntity);

                terrorizeContext.terrorizeAbilitiesList = new List<EcsPackedEntity>();
                terrorizeContext.validAbilitiesList = new List<EcsPackedEntity>();

                foreach (var abilityEntity in abilityUnitComp.AbilityUnitMB.GetAllAbilitiesEntities())
                {
                    ref var abilityComp = ref _abilityPool.Value.Get(abilityEntity);
                    if (abilityComp.Ability.SourceAbility.AbilityType == AbilitySystem.AbilityTypes.Terrorize)
                    terrorizeContext.terrorizeAbilitiesList.Add(_world.Value.PackEntity(abilityEntity));
                }
            }
        }
    }
}
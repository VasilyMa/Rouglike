using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using System.Collections.Generic;
using UnityEngine;

namespace Client {
    sealed class InitFromPlaceContextSystem : MainEcsSystem {
        readonly EcsWorldInject _world = default;
        readonly EcsFilterInject<Inc<UnitBrain, FromPlaceContext, InitContextEvent>> _filter = default;
        readonly EcsPoolInject<FromPlaceContext> _toPointPool = default;
        readonly EcsPoolInject<AbilityUnitComponent> _abilityUnitPool = default;
        readonly EcsPoolInject<AbilityComponent> _abilityPool = default;

        public override MainEcsSystem Clone()
        {
            return new InitFromPlaceContextSystem();
        }

        public override void Run(IEcsSystems systems)
        {
            foreach (int unitEntity in _filter.Value)
            {
                ref var toPointContext = ref _toPointPool.Value.Get(unitEntity);
                ref var abilityUnitComp = ref _abilityUnitPool.Value.Get(unitEntity);

                toPointContext.fromPlaceAbilitiesList = new List<EcsPackedEntity>();
                toPointContext.validAbilitiesList = new List<EcsPackedEntity>();
                toPointContext.fromPlaceTransforms = new List<Transform>();
                var GOs = GameObject.FindGameObjectsWithTag("FromPlaceAction");

                for(int i = 0; i < GOs.Length; i++)
                {
                    toPointContext.fromPlaceTransforms.Add(GOs[i].transform);
                }
                foreach (var abilityEntity in abilityUnitComp.AbilityUnitMB.GetAllAbilitiesEntities())
                {
                    ref var abilityComp = ref _abilityPool.Value.Get(abilityEntity);
                    if (abilityComp.Ability.SourceAbility.AbilityType == AbilitySystem.AbilityTypes.FromPlace)
                        toPointContext.fromPlaceAbilitiesList.Add(_world.Value.PackEntity(abilityEntity));
                }
            }
        }
    }
}
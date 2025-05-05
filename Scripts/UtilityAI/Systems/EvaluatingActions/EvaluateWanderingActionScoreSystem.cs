using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
namespace Client {
    sealed class EvaluateWanderingActionScoreSystem : MainEcsSystem 
    {
        readonly private EcsFilterInject<Inc<UnitBrain, SelfContext, TargetsContext, EvaluationHelpersData>, Exc<InActionComponent, HardControlComponent>> _filter = default;
        readonly private EcsPoolInject<UnitBrain> _brainPool = default;
        readonly private EcsPoolInject<SelfContext> _selfContextPool = default;
        readonly private EcsPoolInject<EvaluationHelpersData> _helpersData = default;
        readonly private EcsPoolInject<TargetsContext> _targetsContext = default;
        readonly private EcsPoolInject<AbilityComponent> _abilityPool = default;
        readonly private EcsWorldInject _world = default;
        float _triggerDistance = 10f;
        public override MainEcsSystem Clone()
        {
            return new EvaluateWanderingActionScoreSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach (int unitEntity in _filter.Value)
            {
                ref var brainComp = ref _brainPool.Value.Get(unitEntity);
                ref var selfContext = ref _selfContextPool.Value.Get(unitEntity);
                ref var targetsContext = ref _targetsContext.Value.Get(unitEntity);
                ref var helpersData = ref _helpersData.Value.Get(unitEntity);

                float score = EvaluateActionScore( ref targetsContext,ref brainComp);
                
                brainComp.statesScore[AIState.Wandering] = score;
            }
        }

        private float EvaluateActionScore(ref TargetsContext targetsContext, ref UnitBrain unitBrain)
        {
            if (targetsContext.closestEnemyDistance > _triggerDistance && !unitBrain.IsTriggeredUnit)
            {
                return 1;
            }
            else
            {
                unitBrain.IsTriggeredUnit = true;
                return 0;
            }

            /*//float attackAvailabilityAttenuation = attacksContext.anyActionAvailable ? 0f : data.CowardiceScoreByHealth.Evaluate(selfContext.healthPercentage);
            //TODOihor add attenuation by unitSpeed and ability cooldowns
            if (attacksContext.anyActionAvailable && attacksContext.anyActionUsable)
            {
                if (unitBrain.bestAttackAvailable.Unpack(_world.Value, out int abialityEntity))
                {
                    ref var abilityComp = ref _abilityPool.Value.Get(abialityEntity);
                    return 1 - abilityComp.Ability.SourceAbility.ProfitabilityDistance.Evaluate(targetsContext.closestEnemyDistance / 100) - data.AggressionScoreByHealth.Evaluate(selfContext.healthPercentage) ;

                }
                return ((data.CowardiceScoreByHealth.Evaluate(selfContext.healthPercentage) + data.CowardiceScoreByDistance.Evaluate(targetsContext.closestEnemyDistance / 10)) / 2);
            }
            if (unitBrain.bestAttackAvailable.Unpack(_world.Value, out int abilityEntity))
            {
                ref var abilityComp = ref _abilityPool.Value.Get(abilityEntity);
                return ((data.CowardiceScoreByHealth.Evaluate(selfContext.healthPercentage) + data.CowardiceScoreByDistance.Evaluate(targetsContext.closestEnemyDistance / 10)) / 2) - (1 - abilityComp.Ability.SourceAbility.ProfitabilityDistance.Evaluate(targetsContext.closestEnemyDistance / 100));

            }
            else
            {
                return ((data.CowardiceScoreByHealth.Evaluate(selfContext.healthPercentage) + data.CowardiceScoreByDistance.Evaluate(targetsContext.closestEnemyDistance / 10)) / 2);
            }*/
        }
    }
}
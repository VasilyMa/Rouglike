using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using UnityEngine.AI;

namespace Client {
    /// <summary>
    /// Evaluates score of moving to target in case entity has attacks that can not be used right now.
    /// </summary>
    sealed class EvaluateAttackIntentionSystem : MainEcsSystem
    {
        readonly private EcsFilterInject<Inc<UnitBrain, AttacksContext, TargetsContext>, Exc<InActionComponent, HardControlComponent,StaticUnitComponent>> _filter = default;
        readonly private EcsWorldInject _world = default;   
        readonly private EcsPoolInject<AttacksContext> _attacksContextPool = default;
        readonly private EcsPoolInject<TargetsContext> _threatContextPool = default;
        readonly private EcsPoolInject<TransformComponent> _transformPool = default;
        readonly private EcsPoolInject<EvaluationHelpersData> _helperPool = default;
        readonly private EcsPoolInject<SelfContext> _selfPool = default;
        readonly private EcsPoolInject<NavMeshComponent> _navmeshPool = default;
        readonly private EcsPoolInject<AbilityComponent> _abilityPool = default;
        readonly private EcsPoolInject<UnitBrain> _unitBrainPool = default;

        public override MainEcsSystem Clone()
        {
            return new EvaluateAttackIntentionSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach (int unitEntity in _filter.Value)
            {
                ref var attacksContext = ref _attacksContextPool.Value.Get(unitEntity);
                ref var transformComp = ref _transformPool.Value.Get(unitEntity);
                ref var unitBrain = ref _unitBrainPool.Value.Get(unitEntity);

                float bestScore = float.MinValue;
                Vector3 position = Vector3.zero;
                Vector3 lookPosition = Vector3.zero;
               
                if (attacksContext.anyActionUsable && attacksContext.anyActionAvailable)
                {
                    ref var threatContext = ref _threatContextPool.Value.Get(unitEntity);
                    ref var helperData = ref _helperPool.Value.Get(unitEntity);
                    ref var selfContext = ref _selfPool.Value.Get(unitEntity);
                    ref var navmeshComp = ref _navmeshPool.Value.Get(unitEntity);

                    foreach (EcsPackedEntity packedAttackEntity in attacksContext.validAbilitiesList)
                    {
                        (float, Vector3,Vector3) scoreAndPosition = EvaluateChasing(packedAttackEntity, ref threatContext, ref helperData, ref transformComp, ref selfContext, ref navmeshComp,ref unitBrain);
                        if (scoreAndPosition.Item1 > bestScore)
                        {
                            bestScore = scoreAndPosition.Item1;
                            lookPosition = scoreAndPosition.Item2;
                            position = scoreAndPosition.Item3;
                        }
                    }
                }
                unitBrain.priorityPointToMove = position;
                unitBrain.priorityPointToLook = lookPosition;
                unitBrain.statesScore[AIState.MoveTo] = bestScore;
            }
        }

        private (float, Vector3,Vector3) EvaluateChasing(EcsPackedEntity packedEntity, ref TargetsContext threatContext,ref EvaluationHelpersData data, ref TransformComponent transform,ref SelfContext selfContext, ref NavMeshComponent navmeshComp, ref UnitBrain unitBrain)
        {
            if (packedEntity.Unpack(_world.Value, out int entity))
            {
                ref var abilityComp = ref _abilityPool.Value.Get(entity);
                float aggresionScore = ((data.AggressionScoreByDistance.Evaluate(threatContext.closestEnemyDistance / 100)+ data.AggressionScoreByHealth.Evaluate(selfContext.healthPercentage))/2);   /*, data.AggressionScoreByAlliesCount.Evaluate("enemy count")*/  // 2 - average
                float cowardiceScore = data.CowardiceScoreByHealth.Evaluate(selfContext.healthPercentage) - abilityComp.Ability.SourceAbility.ProfitabilityDistance.Evaluate(threatContext.closestEnemyDistance / 100);

                float chasingScore = aggresionScore - abilityComp.Ability.SourceAbility.ProfitabilityDistance.Evaluate(threatContext.closestEnemyDistance / 100);

                if (threatContext.closestEnemyEntity.Unpack(_world.Value, out int target))
                {
                    ref var targetTrans = ref _transformPool.Value.Get(target);
                    return (chasingScore, GetLookPoint(ref navmeshComp,ref unitBrain,ref targetTrans), targetTrans.Transform.position);
                }
                return (0, transform.Transform.position, transform.Transform.position);
            }
            else
            {
                return (0, transform.Transform.position, transform.Transform.position);
            }
        }
        private Vector3 GetLookPoint(ref NavMeshComponent navmeshComp, ref UnitBrain unitBrain, ref TransformComponent targetTransform)
        {
            if (navmeshComp.NavMeshAgent.hasPath && navmeshComp.NavMeshAgent.path.corners.Length > 0)
            {
                return navmeshComp.NavMeshAgent.path.corners[0];
            }
            else
            {
                return targetTransform.Transform.position;
            }
        }
    }
}
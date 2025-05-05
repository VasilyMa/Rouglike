using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
namespace Client
{
    /// <summary>
    /// Evaluates score of Defend AIState.
    /// Iterates through all entities with UnitBrain, DefenseContext and ThreatsContext components
    /// </summary>
    sealed class EvaluateDefendActionScoreSystem : MainEcsSystem
    {
        readonly private EcsFilterInject<Inc<UnitBrain, DefenseContext, ThreatsContext>, Exc<InActionComponent, HardControlComponent>> _filter = default;
        readonly private EcsPoolInject<DefenseContext> _defenseContextPool = default;
        readonly private EcsPoolInject<UnitBrain> _unitBrainPool = default;
        readonly private EcsPoolInject<ThreatsContext> _threatContextPool = default;
        readonly private EcsPoolInject<AttacksContext> _attackContextPool = default;
        readonly private EcsPoolInject<SelfContext> _selfContextPool = default;
        readonly private EcsPoolInject<EvaluationHelpersData> _evaluationDataPool = default;

        public override MainEcsSystem Clone()
        {
            return new EvaluateDefendActionScoreSystem();
        }

        public override void Run(IEcsSystems systems)
        {
            foreach (int entity in _filter.Value)
            {
                ref var unitBrain = ref _unitBrainPool.Value.Get(entity);
                ref var defenseContext = ref _defenseContextPool.Value.Get(entity);
                ref var threatContext = ref _threatContextPool.Value.Get(entity);
                ref var attackContext = ref _attackContextPool.Value.Get(entity);
                ref var selfContext = ref _selfContextPool.Value.Get(entity);
                float bestScore = float.MinValue;
                EcsPackedEntity bestActionEntity = new();
                unitBrain.statesScore[AIState.Defend] = bestScore;
                unitBrain.bestDefensiveActionAvailable = bestActionEntity;
                if (!threatContext.isUnderThreat) return;
                if (threatContext.isUnderThreat)
                {
                    ref var evaluationData = ref _evaluationDataPool.Value.Get(entity);
                    foreach (EcsPackedEntity packedActionEntity in defenseContext.defenseActionsList) //think about how to dodge for AI (Dash, Shield)
                    {
                        float score = EvaluateActionScore(ref threatContext, ref evaluationData, ref selfContext);
                        if (score > bestScore)
                        {
                            bestScore = score;
                            bestActionEntity = packedActionEntity;
                        }
                    }
                }
                unitBrain.statesScore[AIState.Defend] = bestScore;
                unitBrain.bestDefensiveActionAvailable = bestActionEntity;

            }
        }

        private float EvaluateActionScore(ref ThreatsContext threatContext, ref EvaluationHelpersData data, ref SelfContext selfContext)
        {
            //TODOihor some evaluation
            var defenceScore = data.DefenceScoreByHealth.Evaluate(selfContext.healthPercentage);
            return defenceScore;
        }
    }
}
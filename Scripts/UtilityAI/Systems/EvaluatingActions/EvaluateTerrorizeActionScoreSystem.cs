using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client {
    sealed class EvaluateTerrorizeActionScoreSystem : MainEcsSystem {
        readonly private EcsFilterInject<Inc<UnitBrain, TargetsContext, TerrorizeContext,AttacksContext, EvaluationHelpersData>, Exc<InActionComponent, HardControlComponent>> _filter = default;
        readonly private EcsWorldInject _world = default;
        readonly private EcsPoolInject<TerrorizeContext> _terrorizeContextPool = default;
        readonly private EcsPoolInject<AttacksContext> _attacksContextPool = default;
        readonly private EcsPoolInject<UnitBrain> _unitBrainPool = default;
        readonly private EcsPoolInject<TargetsContext> _targetsContextPool = default;
        readonly private EcsPoolInject<SelfContext> _selfContextPool = default;
        readonly private EcsPoolInject<EvaluationHelpersData> _evaluationDataPool = default;
        readonly private EcsPoolInject<AbilityComponent> _abilityPool = default;
        readonly private EcsPoolInject<CooldownRecalculationComponent> _cooldownRecalculationPool = default;

        public override MainEcsSystem Clone()
        {
            return new EvaluateTerrorizeActionScoreSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach (int entity in _filter.Value)
            {
                ref var terrorizeContext = ref _terrorizeContextPool.Value.Get(entity);
                ref var brainComp = ref _unitBrainPool.Value.Get(entity);

                float bestScore = float.MinValue;
                EcsPackedEntity bestTerrorize = new();

                if (terrorizeContext.anyActionUsable)
                {
                    ref var targetsContext = ref _targetsContextPool.Value.Get(entity);
                    ref var evaluationData = ref _evaluationDataPool.Value.Get(entity);
                    ref var selfContext = ref _selfContextPool.Value.Get(entity);

                    foreach (EcsPackedEntity packedAttackEntity in terrorizeContext.validAbilitiesList)
                    {
                        float terrorizeScore = EvaluateTerrorize(packedAttackEntity, ref targetsContext, ref evaluationData, ref selfContext);
                        if (terrorizeScore > bestScore)
                        {
                            bestScore = terrorizeScore;
                            bestTerrorize = packedAttackEntity;
                        }
                    }
                }
                brainComp.bestTerrorizeActionAvailable = bestTerrorize;
                brainComp.statesScore[AIState.Terrorize] = bestScore;
            }
        }
        private float EvaluateTerrorize(EcsPackedEntity packedAttackEntity, ref TargetsContext targetsContext, ref EvaluationHelpersData data, ref SelfContext selfContext)
        {
            if (packedAttackEntity.Unpack(_world.Value, out int abilityEntity))
            {
                ref var abilityComp = ref _abilityPool.Value.Get(abilityEntity);

                float maxDistance = 100;

                float normalizedDistance = Mathf.Clamp01(targetsContext.closestEnemyDistance / maxDistance);

                float abilityScore = abilityComp.Ability.SourceAbility.ProfitabilityDistance.Evaluate(normalizedDistance);
                return abilityScore;
            }
            else
            {
                return 0f;
            }
        }
    }
}
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
namespace Client {
    /// <summary>
    /// System that evaluates support action score.
    /// Constantly iterates through all entities with UnitBrain, SupportContext and AlliesContext components
    /// </summary>
    sealed class EvaluateSupportActionSystem : MainEcsSystem 
    {
        readonly private EcsFilterInject<Inc<UnitBrain, SupportContext, AlliesContext, EvaluationHelpersData>> _filter = default;
        readonly private EcsPoolInject<SupportContext> _supportContextPool = default;
        readonly private EcsPoolInject<AlliesContext> _alliesContextPool = default;
        readonly private EcsPoolInject<UnitBrain> _unitBrainPool = default;
        readonly private EcsPoolInject<EvaluationHelpersData> _dataPool = default;

        public override MainEcsSystem Clone()
        {
            return new EvaluateSupportActionSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach (int unitEntity in _filter.Value)
            {
                ref var supportContext = ref _supportContextPool.Value.Get(unitEntity);
                EcsPackedEntity bestActionEntity = new();
                float bestScore = float.MinValue;
                ref var unitBrain = ref _unitBrainPool.Value.Get(unitEntity);
                if (supportContext.anyActionUsable)
                {
                    ref var alliesContext = ref _alliesContextPool.Value.Get(unitEntity);
                    ref var evaluationData = ref _dataPool.Value.Get(unitEntity);
                    foreach (EcsPackedEntity packedEntity in supportContext.supportAbilitiesList)
                    {
                        float score = EvaluateSupportAction(packedEntity, ref alliesContext, ref evaluationData);
                        if (score > bestScore)
                        {
                            bestScore = score;
                            bestActionEntity = packedEntity;
                        }
                    }
                }
                unitBrain.statesScore[AIState.Support] = bestScore;
                unitBrain.bestSupportActionAvailable = bestActionEntity;
            }
        }

        private float EvaluateSupportAction(EcsPackedEntity packedEntity, ref AlliesContext alliesContext, ref EvaluationHelpersData data)
        {
            //TODOihor some evaluation
            return 1f;
        }
    }
}
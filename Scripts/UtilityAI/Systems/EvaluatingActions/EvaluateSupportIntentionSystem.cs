using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
namespace Client {
    /// <summary>
    /// 
    /// </summary>
    sealed class EvaluateSupportIntentionSystem : MainEcsSystem
    {
        readonly EcsFilterInject<Inc<UnitBrain, SupportContext, AlliesContext>, Exc<InActionComponent, HardControlComponent>> _filter = default;
        readonly private EcsPoolInject<SupportContext> _supportContextPool = default;
        readonly private EcsPoolInject<SelfContext> _selfContextPool = default;
        readonly private EcsPoolInject<AlliesContext> _alliesContextPool = default;
        readonly private EcsPoolInject<UnitBrain> _unitBrainPool = default;

        public override MainEcsSystem Clone()
        {
            return new EvaluateSupportIntentionSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach (int entity in _filter.Value)
            {
                foreach (int unitEntity in _filter.Value)
                {
                    ref var supportContext = ref _supportContextPool.Value.Get(unitEntity);
                    ref var unitBrain = ref _unitBrainPool.Value.Get(unitEntity);
                    float bestScore = float.MinValue;
                    Vector3 position = Vector3.zero;
                    if (!supportContext.anyActionUsable && supportContext.anyActionAvailable)
                    {
                        ref var alliesContext = ref _alliesContextPool.Value.Get(unitEntity);
                        foreach (EcsPackedEntity packedSupportActionEntity in supportContext.supportAbilitiesList)
                        {
                            (float, Vector3) scoreAndPosition = EvaluateChasing(packedSupportActionEntity, ref alliesContext);
                            if (scoreAndPosition.Item1 > bestScore)
                            {
                                bestScore = scoreAndPosition.Item1;
                                position = scoreAndPosition.Item2;
                            }
                        }
                    }
                    if (bestScore > unitBrain.statesScore[AIState.MoveTo])
                    {
                        unitBrain.priorityPointToMove = position;
                        unitBrain.statesScore[AIState.MoveTo] = bestScore;
                    }
                }
            }
        }
        private (float, Vector3) EvaluateChasing(EcsPackedEntity packedEntity, ref AlliesContext threatContext)
        {
            //TODOihor some evaluation
            Vector3 somePosition = new();
            return (0f, somePosition);
        }
    }
}
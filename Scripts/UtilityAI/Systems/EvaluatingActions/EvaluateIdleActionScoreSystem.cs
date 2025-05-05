using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
namespace Client {
    sealed class EvaluateIdleActionScoreSystem : MainEcsSystem 
    {
        readonly EcsFilterInject<Inc<UnitBrain, EvaluationHelpersData>> _filter = default;
        readonly EcsPoolInject<UnitBrain> _brainPool = default;
        readonly EcsPoolInject<EvaluationHelpersData> _evaluationData = default;

        public override MainEcsSystem Clone()
        {
            return new EvaluateIdleActionScoreSystem();
        }

        public override void Run (IEcsSystems systems) {
            // system for future logic expanding, it does nothing for now
            foreach (int entity in _filter.Value)
            {
                ref var brainComp = ref _brainPool.Value.Get(entity);
                brainComp.statesScore[AIState.Idle] = 0.25f;   // for tests POCHEMU TAK
            }
        }
    }
}
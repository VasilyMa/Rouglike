using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
namespace Client {
    sealed class InitEvaluationHelpersData : MainEcsSystem {
        readonly EcsFilterInject<Inc<UnitBrain, InitAIEvent, EvaluationHelpersData>> _eventFilter = default;
        readonly EcsPoolInject<EvaluationHelpersData> _evaluationDataPool = default;
        readonly EcsPoolInject<InitAIEvent> _initAIPool;

        public override MainEcsSystem Clone()
        {
            return new InitEvaluationHelpersData();
        }

        //readonly EcsPoolInject<CreateEnemyEvent> _createPool = default;

        public override void Run(IEcsSystems systems) {
            foreach (int entity in _eventFilter.Value)
            {
                //TODOihor get curves data somehwere else
                /*ref var evaluationData = ref _evaluationDataPool.Value.Get(entity);
                evaluationData.idleRating = Random.Range(0f, 0.25f);
                evaluationData.cowardiceRating = Random.Range(0.05f, .1f);
                evaluationData.smartRating = Random.Range(.7f, 1f);
                evaluationData.healthCurve = new AnimationCurve(new Keyframe(0f, 1f), new Keyframe(1f, 0f));
                evaluationData.distanceCurve = new AnimationCurve(new Keyframe(10f, 0f), new Keyframe(0f, 1f));*/

                ref var evaluationHelperData = ref _evaluationDataPool.Value.Get(entity);
                ref var AIProfile = ref _initAIPool.Value.Get(entity).AIprofile;

                evaluationHelperData.AggressionScoreByHealth =  AIProfile.AggressionScoreByHealth;
                evaluationHelperData.AggressionScoreByDistance = AIProfile.AggressionScoreByDistance;

                evaluationHelperData.DefenceScoreByHealth = AIProfile.DefenceScoreByHealth;

                evaluationHelperData.CowardiceScoreByHealth = AIProfile.CowardiceScoreByHealth;
                evaluationHelperData.CowardiceScoreByDistance = AIProfile.CowardiceScoreByDistance;

                evaluationHelperData.ReactionToAction = AIProfile.ReactionToAction;

                evaluationHelperData.ReactionDelay = AIProfile.ReactionToAction;
            }
        }
    }
}
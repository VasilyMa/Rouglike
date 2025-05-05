using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using System.Collections.Generic;

namespace Client {
    /// <summary>
    /// Here happens utility AI brain initialization. 
    /// It needs Brain monobehaviour on unit in order to be initialized.
    /// It also should happen after all abilities have been initialized.
    /// </summary>
    sealed class InitUnitBrainSystem : MainEcsSystem {
        readonly private EcsFilterInject<Inc<InitAIEvent, ViewComponent>> _eventFilter = default;

        readonly EcsPoolInject<ViewComponent> _viewPool = default;

        readonly EcsPoolInject<UnitBrain> _brainPool = default;

        readonly EcsPoolInject<EvaluationHelpersData> _evaluationData = default;

        readonly EcsPoolInject<AttacksContext> _attacksContextPool = default;
        readonly EcsPoolInject<TargetsContext> _targetsContextPool = default;
        readonly EcsPoolInject<SelfContext> _selfContextPool = default;
        readonly EcsPoolInject<AlliesContext> _alliesContextPool = default;
        readonly EcsPoolInject<DefenseContext> _defenseContextPool = default;
        readonly EcsPoolInject<SupportContext> _supportContextPool = default;
        readonly EcsPoolInject<ThreatsContext> _threatsContextPool = default;
        readonly EcsPoolInject<TerrorizeContext> _terrorizePool = default;
        readonly EcsPoolInject<FromPlaceContext> _toPointPool = default;

        public override MainEcsSystem Clone()
        {
            return new InitUnitBrainSystem();
        }

        public override void Run (IEcsSystems systems) {

            foreach (int unitEntity in _eventFilter.Value)
            {
                ref var unitView = ref _viewPool.Value.Get(unitEntity);
                //if (!unitView.GameObject.TryGetComponent(out Brain brain)) continue;
                ref var brainComp = ref _brainPool.Value.Add(unitEntity);
                brainComp.statesScore = new Dictionary<AIState, float>();
                foreach (AIState state in System.Enum.GetValues(typeof(AIState)))
                {
                    brainComp.statesScore.Add(state, 0f);
                }
                //TODOihor context must be initialized based on some conditions

                // general context
                _selfContextPool.Value.Add(unitEntity);
                // context if unit has any support actions
                _alliesContextPool.Value.Add(unitEntity);
                _supportContextPool.Value.Add(unitEntity);

                // context if unit has any attack actions
                _attacksContextPool.Value.Add(unitEntity);
                _targetsContextPool.Value.Add(unitEntity);

                // context if unit has any defensive actions
                _defenseContextPool.Value.Add(unitEntity);
                _threatsContextPool.Value.Add(unitEntity);
                _terrorizePool.Value.Add(unitEntity);
                _toPointPool.Value.Add(unitEntity);

                _evaluationData.Value.Add(unitEntity);

            }
        }
    }
}
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class EvaluateFromPlaceScoreSystem : MainEcsSystem 
    {
        readonly private EcsFilterInject<Inc<UnitBrain, FromPlaceContext>,Exc<InActionComponent, HardControlComponent>> _filter = default;
        readonly private EcsWorldInject _world = default;
        readonly private EcsPoolInject<FromPlaceContext> fromPlaceContextPool = default;
        readonly private EcsPoolInject<TargetsContext> _threatContextPool = default;
        readonly private EcsPoolInject<TransformComponent> _transformPool = default;
        readonly private EcsPoolInject<EvaluationHelpersData> _helperPool = default;
        readonly private EcsPoolInject<SelfContext> _selfPool = default;
        readonly private EcsPoolInject<NavMeshComponent> _navmeshPool = default;
        readonly private EcsPoolInject<AbilityComponent> _abilityPool = default;
        readonly private EcsPoolInject<UnitBrain> _unitBrainPool = default;

        public override MainEcsSystem Clone()
        {
            return new EvaluateFromPlaceScoreSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach (var unitEntity in _filter.Value)
            {
                ref var fromPlaceContext = ref fromPlaceContextPool.Value.Get(unitEntity);
                ref var transformComp = ref _transformPool.Value.Get(unitEntity);
                ref var unitBrain = ref _unitBrainPool.Value.Get(unitEntity);
                if (fromPlaceContext.AnyActionUsable)
                {
                    unitBrain.priorityPointToMove = fromPlaceContext.fromPlaceTransforms[0].position;
                    unitBrain.priorityPointToLook = fromPlaceContext.fromPlaceTransforms[0].position;
                    unitBrain.statesScore[AIState.MoveTo] = 1;
                }
            }
        }
    }
}
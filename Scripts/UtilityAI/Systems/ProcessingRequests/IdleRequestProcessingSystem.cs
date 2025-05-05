using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client {
    /// <summary>
    /// System proccesses all idle request events.
    /// </summary>
    sealed class IdleRequestProcessingSystem : MainEcsSystem
    {
        readonly private EcsFilterInject<Inc<IdleRequest, UnitBrain>,Exc<InActionComponent,HardControlComponent>> _filter = default;
        readonly private EcsFilterInject<Inc<PlayerComponent>,Exc<DeadComponent>> _playerFilter = default;
        readonly private EcsPoolInject<UnitBrain> _brainPool = default;
        readonly private EcsPoolInject<TransformComponent> _transformPool = default;
        readonly private EcsPoolInject<IdleRequest> _idlerequestPool = default;
        readonly private EcsPoolInject<StopNavigationRequest> _stopNavigationRequestPool = default;
        readonly private EcsWorldInject _world = default;
        readonly EcsPoolInject<IdleAnimationState> _idleAnimationPool = default;

        public override MainEcsSystem Clone()
        {
            return new IdleRequestProcessingSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach (int unitEntity in _filter.Value)
            {
                // request for navMesh agent stopping
                ref var unitBrain = ref _brainPool.Value.Get(unitEntity);

                if (unitBrain.CurrentState != AIState.Idle)
                {
                    _idlerequestPool.Value.Del(unitEntity);
                    continue;
                }
                ref var stopNavigationRequest = ref _stopNavigationRequestPool.Value.Add(_world.Value.NewEntity());
                stopNavigationRequest.packedEntity = _world.Value.PackEntity(unitEntity);
                foreach (var player in _playerFilter.Value)
                {
                     ref var trans = ref _transformPool.Value.Get(player);
                    unitBrain.priorityPointToLook = trans.Transform.position;
                }
                // request for idle animation
                _idleAnimationPool.Value.Add(unitEntity);
                //ChangeAnimationController.ChangeAnimationFunc(AnimationTypes.Idle, unitEntity);
            }
        }
    }
}
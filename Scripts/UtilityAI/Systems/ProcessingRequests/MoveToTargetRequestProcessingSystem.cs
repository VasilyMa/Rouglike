using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
namespace Client {
    sealed class MoveToTargetRequestProcessingSystem : MainEcsSystem
    {
        readonly private EcsFilterInject<Inc<UnitBrain, MoveToTargetRequest>, Exc<InActionComponent, HardControlComponent>> _filter = default;
        readonly private EcsPoolInject<StartNavigationRequest> _startNavigationRequestPool = default;
        readonly private EcsWorldInject _world = default;

        readonly EcsPoolInject<MoveAnimationState> _moveAnimationPool = default;

        public override MainEcsSystem Clone()
        {
            return new MoveToTargetRequestProcessingSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach (int unitEntity in _filter.Value)
            {
                // request navmesh enabling
                ref var startNavigationRequest = ref _startNavigationRequestPool.Value.Add(_world.Value.NewEntity());
                startNavigationRequest.packedEntity = _world.Value.PackEntity(unitEntity);

                // request animation switch
                //ChangeAnimationController.ChangeAnimationFunc(AnimationTypes.Move, unitEntity);
                _moveAnimationPool.Value.Add(unitEntity);
            }
        }
    }
}
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class SupportRequestProcessingSystem : MainEcsSystem
    {
        readonly private EcsFilterInject<Inc<UnitBrain, SupportRequest>> _filter = default;
        readonly private EcsPoolInject<UnitBrain> _brainPool = default;
        readonly private EcsPoolInject<StopNavigationRequest> _stopNavigationRequestPool = default;
        readonly private EcsWorldInject _world;

        public override MainEcsSystem Clone()
        {
            return new SupportRequestProcessingSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach (int unitEntity in _filter.Value)
            {
                // request navMesh agent stop
                ref var stopNavigationRequest = ref _stopNavigationRequestPool.Value.Add(_world.Value.NewEntity());
                stopNavigationRequest.packedEntity = _world.Value.PackEntity(unitEntity);
                // request ability sequence start
                ref var unitBrain = ref _brainPool.Value.Get(unitEntity);
                //unitBrain.bestSupportActionAvailable => best support action entity
                
            }
        }
    }
}
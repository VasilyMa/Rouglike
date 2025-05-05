using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
namespace Client {
    sealed class DefendRequestProcessingSystem : MainEcsSystem
    {
        readonly private EcsFilterInject<Inc<UnitBrain, DefendRequest>,Exc<DefenseTag>> _filter = default;
        readonly private EcsPoolInject<ThreatsContext> _threatsContextPool = default;
        readonly private EcsPoolInject<UnitBrain> _brainPool = default;
        readonly private EcsPoolInject<DefenseTag> _defenseTagPool = default;
        readonly private EcsPoolInject<StopNavigationRequest> _stopNavigationRequestPool = default;
        readonly private EcsWorldInject _world = default;
        public override MainEcsSystem Clone()
        {
            return new DefendRequestProcessingSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach (int unitEntity in _filter.Value)
            {
                // request for navmeshAgent stop
                /*ref var stopnavigationRequest = ref _stopNavigationRequestPool.Value.Add(_world.Value.NewEntity());
                stopnavigationRequest.packedEntity = _world.Value.PackEntity(unitEntity);*/
                // decide in which direction unit should dodge
                ref var brainComp = ref _brainPool.Value.Get(unitEntity);
                ref var threatsContext = ref _threatsContextPool.Value.Get(unitEntity);
                _defenseTagPool.Value.Add(unitEntity);

                

                // request for ability sequence start
                // brainComp.bestDefensiveActionAvailable  => best support ability entity

                // request rotation if needed ??
            }
        }
    }
}
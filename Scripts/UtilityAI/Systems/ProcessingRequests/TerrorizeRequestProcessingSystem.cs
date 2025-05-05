using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class TerrorizeRequestProcessingSystem : MainEcsSystem
    {
        readonly private EcsFilterInject<Inc<TerrorizeRequest, UnitBrain>> _filter = default;
        readonly private EcsPoolInject<StopNavigationRequest> _stopNavigationRequestPool = default;
        readonly private EcsPoolInject<UnitBrain> _unitBrainPool = default;
        readonly private EcsPoolInject<TargetsContext> _targetsContext = default;
        readonly private EcsPoolInject<TerrorizeTag> _terrorizeTagPool = default;
        readonly private EcsWorldInject _world = default;

        public override MainEcsSystem Clone()
        {
            return new TerrorizeRequestProcessingSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach (int unitEntity in _filter.Value)
            {
                
                ref var stopRequest = ref _stopNavigationRequestPool.Value.Add(_world.Value.NewEntity());
                stopRequest.packedEntity = _world.Value.PackEntity(unitEntity);
                
                ref var brainComp = ref _unitBrainPool.Value.Get(unitEntity);
                ref var targetsContext = ref _targetsContext.Value.Get(unitEntity);
               
                if (targetsContext.closestEnemyEntity.Unpack(_world.Value, out int enemyEntity))
                {
                        if (!_terrorizeTagPool.Value.Has(unitEntity))
                        {
                            _terrorizeTagPool.Value.Add(unitEntity); 
                        }
                }
            }
        }
    }
}
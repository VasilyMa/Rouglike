using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class SpecialStaticUnitInit : MainEcsSystem {
        readonly EcsFilterInject<Inc<SpawnStaticUnitEvent>> _filter;
        readonly EcsPoolInject<SpawnStaticUnitEvent> _spawnStaticUnitPool;
        readonly EcsPoolInject<InitContextEvent> _initContextPool = default;
        readonly EcsPoolInject<InitAIEvent> _initAIPool;
        public override MainEcsSystem Clone()
        {
            return new SpecialStaticUnitInit();
        }
        public override void Run (IEcsSystems systems) {
            foreach(var entity in _filter.Value)
            {
                //ref var spawnComponent = ref _spawnStaticUnitPool.Value.Get(entity);
                //_initAIPool.Value.Add(entity).AIprofile = spawnComponent.AIprofile;
                //_initContextPool.Value.Add(entity);
            }
        }
    }
}
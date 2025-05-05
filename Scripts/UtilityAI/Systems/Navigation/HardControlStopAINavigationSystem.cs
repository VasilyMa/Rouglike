using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class HardControlStopAINavigationSystem : MainEcsSystem
    {
        readonly EcsWorldInject _world = default;
        readonly EcsFilterInject<Inc<AddHardControlEvent>,Exc<PlayerComponent>> _filter = default;
        readonly EcsPoolInject<PlayerComponent> _playerPool = default;
        readonly EcsPoolInject<StopNavigationRequest> _stopNavigationRequestPool = default;

        public override MainEcsSystem Clone()
        {
            return new HardControlStopAINavigationSystem();
        }

        public override void Run (IEcsSystems systems) {
            // add your run code here.
            foreach(var entity in _filter.Value)
            {
               _stopNavigationRequestPool.Value.Add(_world.Value.NewEntity()).packedEntity = _world.Value.PackEntity(entity);
            }
        }
    }
}
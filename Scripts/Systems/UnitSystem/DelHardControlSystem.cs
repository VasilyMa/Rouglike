using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class DelHardControlSystem : MainEcsSystem 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsFilterInject<Inc<DelHardControlEvent, HardControlComponent>> _filter = default;
        readonly EcsPoolInject<HardControlComponent> _hardControlPool = default;
        readonly EcsPoolInject<HardControlTimerComponent> _timerPool = default;
        readonly EcsPoolInject<RequestActiveControlEvent> _requestPool = default;
        readonly EcsPoolInject<StartNavigationRequest> _startNavigationRequest = default;
        readonly EcsPoolInject<PlayerComponent> _playerPool = default;

        public override MainEcsSystem Clone()
        {
            return new DelHardControlSystem();
        }

        public override void Run (IEcsSystems systems) 
        {
            foreach(var entity in _filter.Value)
            {
                _hardControlPool.Value.Del(entity);
                _timerPool.Value.Del(entity);
                //todo кинуть евент на снятие лока перемещения и поворота

                ref var requestComp = ref _requestPool.Value.Add(_world.Value.NewEntity());
                requestComp.TargetEntity = _world.Value.PackEntity(entity);
                
            }
        }
    }
}
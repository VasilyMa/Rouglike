using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class RequestActiveControlSystem : MainEcsSystem
    {
        readonly EcsWorldInject _world = default;
        readonly EcsFilterInject<Inc<RequestActiveControlEvent>> _filter = default;
        readonly EcsPoolInject<RequestActiveControlEvent> _requestPool = default;
        readonly EcsPoolInject<ActiveControlEvent> _activeControlPool = default;
        readonly EcsPoolInject<HardControlComponent> _hardControlPool = default;
        readonly EcsPoolInject<InActionComponent> _inActionPool = default;

        public override MainEcsSystem Clone()
        {
            return new RequestActiveControlSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach(var entity in _filter.Value)
            {
                ref var requestComp = ref _requestPool.Value.Get(entity);
                if(requestComp.TargetEntity.Unpack(_world.Value, out int targetEntity))
                {
                    if(_hardControlPool.Value.Has(targetEntity)) continue;
                    if(_inActionPool.Value.Has(targetEntity)) continue;
                    if(!_activeControlPool.Value.Has(targetEntity)) _activeControlPool.Value.Add(targetEntity);
                }
            }
        }
    }
}
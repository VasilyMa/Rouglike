using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class RequestLockMoveEventSystem : MainEcsSystem
    { 
        readonly EcsWorldInject _world = default;
        readonly EcsFilterInject<Inc<RequestLockMoveEvent>> _filter = default;
        readonly EcsPoolInject<RequestLockMoveEvent> _requestPool = default;
        readonly EcsPoolInject<LockMoveComponent> _lockMovePool = default;

        public override MainEcsSystem Clone()
        {
            return new RequestLockMoveEventSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach(var entity in _filter.Value)
            {
                ref var requestComp = ref _requestPool.Value.Get(entity);
                if(requestComp.TargetPackedEntity.Unpack(_world.Value, out int targetEntity))
                {
                    if(!_lockMovePool.Value.Has(targetEntity)) _lockMovePool.Value.Add(targetEntity);
                }
            }
        }
    }
}
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client {
    sealed class RequestLockRotationEventSystem : MainEcsSystem 
    { 
        readonly EcsWorldInject _world = default;
        readonly EcsFilterInject<Inc<RequestLockRotationEvent>> _filter = default;
        readonly EcsPoolInject<RequestLockRotationEvent> _requestPool = default;
        readonly EcsPoolInject<LockRotationComponent> _lockRotationPool = default;

        public override MainEcsSystem Clone()
        {
            return new RequestLockRotationEventSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach(var entity in _filter.Value)
            {
                ref var requestComp = ref _requestPool.Value.Get(entity);
                if(requestComp.TargetPackedEntity.Unpack(_world.Value, out int targetEntity))
                {
                    if(!_lockRotationPool.Value.Has(targetEntity))
                    {   
                        _lockRotationPool.Value.Add(targetEntity);
                        //if(targetEntity != GameState.Instance.PlayerEntity) Debug.Break();
                    } 
                }
            }
        }
    }
}
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using AbilitySystem;

namespace Client {
    sealed class RequestMissileAbilitySystem : MainEcsSystem
    {
        readonly EcsWorldInject _world = default;
        readonly EcsFilterInject<Inc<RequestMissileAbilityEvent>> _filter = default;
        readonly EcsPoolInject<RequestMissileAbilityEvent> _requestPool = default;
        readonly EcsPoolInject<InvokeMissileEvent> _invokePool = default;

        public override MainEcsSystem Clone()
        {
            return new RequestMissileAbilitySystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach(var entity in _filter.Value)
            {
                ref var requestComp = ref _requestPool.Value.Get(entity);
                if(requestComp.OwnerEntity.Unpack(_world.Value, out int ownerEntity))
                {
                    //if(!_invokePool.Value.Has(ownerEntity))
                    //{
                        ref var invokeComp = ref _invokePool.Value.Add(entity);
                        invokeComp.Init(requestComp.AbilityPackedEntity, requestComp.OwnerEntity, requestComp.Components);
                        invokeComp.missile = requestComp.missile;
                        invokeComp.Offset = requestComp.Offset; 
                        invokeComp.Speed = requestComp.Speed; 
                   // }
                }
            }
        }
    }
}
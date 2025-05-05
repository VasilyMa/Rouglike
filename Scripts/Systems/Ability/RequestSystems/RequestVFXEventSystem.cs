using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class RequestVFXEventSystem : MainEcsSystem
    { //get request and check for valid, if valid => give vfx data and invoke vfx
        readonly EcsWorldInject _world = default;
        readonly EcsFilterInject<Inc<RequestVFXEvent>> _requestFilter = default;
        readonly EcsPoolInject<RequestVFXEvent> _requestPool = default;
        readonly EcsPoolInject<InvokeVisualEffectEvent> _invokePool = default;

        public override MainEcsSystem Clone()
        {
            return new RequestVFXEventSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach (var entity in _requestFilter.Value)
            {
                ref var requestComp = ref _requestPool.Value.Get(entity);
                if (requestComp.TargetPackedEntity.Unpack(_world.Value, out int targetEntity))
                {
                    foreach(var _sourceParticle in requestComp.sourceParticle)
                    {
                        ref var invokeComp = ref _invokePool.Value.Add(_world.Value.NewEntity());
                        invokeComp.RotationOffset = requestComp.RotationOffset;
                        invokeComp.offset =  requestComp.offset;
                        invokeComp.IsParentTransformInvoke = requestComp.IsParentTransformInvoke;
                        invokeComp.Particle = _sourceParticle;
                        invokeComp.EntityCaster = requestComp.TargetPackedEntity;
                        invokeComp.AbilityEntity = requestComp.AbilityEntity;
                    }
                    
                }
            }
        }
    }
}
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client {
    sealed class RequestAttackZoneEventSystem : MainEcsSystem 
    { //get request and check for valid, if valid => throw attackzone event on entity
        readonly EcsWorldInject _world = default;
        readonly EcsFilterInject<Inc<RequestAttackZoneEvent>> _requestFilter = default;
        readonly EcsPoolInject<RequestAttackZoneEvent> _requestPool = default;
        readonly EcsPoolInject<InvokeAttackZoneEvent> _invokeAttackZonePool = default;

        public override MainEcsSystem Clone()
        {
            return new RequestAttackZoneEventSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach (var entity in _requestFilter.Value)
            {
                ref var requestComp = ref _requestPool.Value.Get(entity);
                if (requestComp.TargetPackedEntity.Unpack(_world.Value, out int targetEntity))
                {
                    if(!_invokeAttackZonePool.Value.Has(targetEntity))
                    {
                        ref var invokeComp = ref _invokeAttackZonePool.Value.Add(targetEntity);
                        invokeComp.Init(requestComp.AbilityEntity, requestComp.TargetPackedEntity, requestComp.AttackMesh, requestComp.Size, requestComp.DisableTime);
                    }
                }
            }
        }
    }
}
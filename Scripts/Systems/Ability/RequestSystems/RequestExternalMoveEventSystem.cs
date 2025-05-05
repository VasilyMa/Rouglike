using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    sealed class RequestExternalMoveEventSystem : MainEcsSystem
    {
        readonly EcsWorldInject _world = default;
        readonly EcsFilterInject<Inc<RequestExternalMoveEvent>> _filter = default;
        readonly EcsPoolInject<RequestExternalMoveEvent> _requestPool = default;
        readonly EcsPoolInject<ExternalMoveComponent> _externalPool = default;
        readonly EcsPoolInject<TransformComponent> _transformPool = default;

        public override MainEcsSystem Clone()
        {
            return new RequestExternalMoveEventSystem();
        }

        public override void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref var requestComp = ref _requestPool.Value.Get(entity);
                if (requestComp.OwnerPackedEntity.Unpack(_world.Value, out int ownerEntity))
                {
                    ref var transformMoveComp = ref _transformPool.Value.Get(ownerEntity);

                    if (!_externalPool.Value.Has(ownerEntity))
                    {
                        ref var externalMoveComp = ref _externalPool.Value.Add(ownerEntity);
                        externalMoveComp.Invoke(requestComp.MoveDirection, requestComp.ForceMode, requestComp.Speed, requestComp.Duration, isInterruptible: requestComp.IsInterruptible,directionType: requestComp.Direction);
                    }
                    else
                    {
                        ref var externalMoveComp = ref _externalPool.Value.Get(ownerEntity);
                        externalMoveComp.Invoke(requestComp.MoveDirection, requestComp.ForceMode, Mathf.Max(externalMoveComp.Speed, requestComp.Speed), Mathf.Max(requestComp.Duration, externalMoveComp.Duration), externalMoveComp.MoveDirection, isInterruptible: requestComp.IsInterruptible, directionType: requestComp.Direction);
                    }
                }
            }
        }
    }
}
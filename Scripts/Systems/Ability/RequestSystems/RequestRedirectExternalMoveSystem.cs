using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using Statement;

namespace Client
{
    sealed class RequestRedirectExternalMoveSystem : MainEcsSystem   //redirect moveDir by dirType
    {
        readonly EcsFilterInject<Inc<WASDInputEvent>> _inputFilter = default;
        readonly EcsWorldInject _world = default;
        readonly EcsPoolInject<WASDInputEvent> _wasdPool = default;
        readonly EcsFilterInject<Inc<RequestExternalMoveEvent>> _filter = default;
        readonly EcsPoolInject<RequestExternalMoveEvent> _requestPool = default;
        readonly EcsPoolInject<TransformComponent> _transformPool = default;
        readonly EcsPoolInject<TargetComponent> _targetPool = default;
        readonly EcsPoolInject<PlayerComponent> _playerPool = default;
        readonly EcsPoolInject<VisualEffectsComponent> _visualEffectPool = default;

        public override MainEcsSystem Clone()
        {
            return new RequestRedirectExternalMoveSystem();
        }

        public override void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref var requestComp = ref _requestPool.Value.Get(entity);

                if (requestComp.OwnerPackedEntity.Unpack(_world.Value, out int ownerEntity))
                {
                    ref var transformComp = ref _transformPool.Value.Get(ownerEntity);
                    #region Forward
                    if (requestComp.Direction == RequestExternalMoveEvent.DirectionType.Forward)
                    {
                        requestComp.MoveDirection = transformComp.Transform.forward;
                    }
                    #endregion 
                    #region WASD
                    else if (requestComp.Direction == RequestExternalMoveEvent.DirectionType.WASD && ownerEntity == State.Instance.GetEntity("PlayerEntity"))
                    {
                        foreach (var inputEntity in _inputFilter.Value)
                        {
                            ref var wasdComp = ref _wasdPool.Value.Get(inputEntity);
                            requestComp.MoveDirection = wasdComp.WasdDirection;
                        }
                        if (_inputFilter.Value.GetEntitiesCount() < 1)
                        {
                            requestComp.MoveDirection = transformComp.Transform.forward;
                        }
                    }
                    #endregion 
                    #region ToTarget
                    else if (requestComp.Direction == RequestExternalMoveEvent.DirectionType.ToTarget && ownerEntity != State.Instance.GetEntity("PlayerEntity"))
                    {
                        ref var targetComp = ref _targetPool.Value.Get(ownerEntity);
                        if (targetComp.TargetPackedEntity.Unpack(_world.Value, out int targetEntity))
                        {
                            ref var targetTransformComp = ref _transformPool.Value.Get(targetEntity);
                            var dir = (targetTransformComp.Transform.position - transformComp.Transform.position).normalized;
                            var extraDistance = dir * requestComp.ExtraDistance;
                            requestComp.MoveDirection = targetTransformComp.Transform.position - transformComp.Transform.position + extraDistance;
                        }
                    }
                    #endregion 
                    if(_playerPool.Value.Has(ownerEntity))
                    {
                        ref var visualEffectComp = ref _visualEffectPool.Value.Get(ownerEntity);
                        visualEffectComp.DashParticle.SetVelocityProperty(requestComp.MoveDirection);
                    } 
                }
            }
        }
    }
}
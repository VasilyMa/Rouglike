using Leopotam.EcsLite;
using UnityEngine;
using Leopotam.EcsLite.Di;
using Statement;

namespace Client {
    sealed class AddRotationSystem : MainEcsSystem 
    {
        readonly EcsFilterInject<Inc<EnemyComponent, TargetComponent>, Exc<DeadComponent, LockRotationComponent, StunComponent, Circling>> _filter = default;
        readonly EcsPoolInject<RotationComponent> _rotationPool = default;
        readonly EcsPoolInject<TargetComponent> _targetPool = default;
        readonly EcsPoolInject<TransformComponent> _transfromPool = default;
        readonly EcsPoolInject<MoveComponent> _movePool = default;

        public override MainEcsSystem Clone()
        {
            return new AddRotationSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach (var entity in _filter.Value)
            {
                
                ref var transformComp = ref _transfromPool.Value.Get(entity);
                ref var targetComp = ref _targetPool.Value.Get(entity);

                var world = State.Instance.EcsRunHandler.World;

                if (targetComp.TargetPackedEntity.Unpack(world, out int targetEntity))
                {
                    var targetPoint = Vector3.zero;
                    if(_movePool.Value.Has(entity))
                    {
                        ref var moveComp = ref _movePool.Value.Get(entity);
                        targetPoint = moveComp.TargetPosition;
                    }
                    else
                    {
                        ref var targetViewComp = ref _transfromPool.Value.Get(targetEntity);
                        targetPoint = targetViewComp.Transform.position;
                    }

                    var direction = (targetPoint - transformComp.Transform.position).normalized;
                    ref var rotationComp = ref _rotationPool.Value.Add(entity);
                    rotationComp.ViewDirection = direction;
                }
            }
        }
    }
}
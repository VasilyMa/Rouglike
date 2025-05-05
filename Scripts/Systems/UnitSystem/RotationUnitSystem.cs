using Leopotam.EcsLite;
using UnityEngine;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class RotationUnitSystem : MainEcsSystem
    {
        readonly EcsPoolInject<TransformComponent> _transfromPool = default;
        readonly EcsFilterInject<Inc<RotationComponent, PlayerComponent>, Exc<DeadComponent>> _filter = default;
        readonly EcsPoolInject<RotationComponent> _rotationPool = default;
        readonly EcsPoolInject<PlayerComponent> _playerPool = default;
        readonly EcsPoolInject<MoveComponent> _movePool = default;
        readonly EcsPoolInject<AnimatorComponent> _animatorPool = default;

        public override MainEcsSystem Clone()
        {
            return new RotationUnitSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach(var entity in _filter.Value)
            {
                ref var rotationComp = ref _rotationPool.Value.Get(entity);
                ref var transfromComponent = ref _transfromPool.Value.Get(entity);
                ref var animatorComponent = ref _animatorPool.Value.Get(entity);
                transfromComponent.Transform.LookAt(transfromComponent.Transform.position + rotationComp.ViewDirection);
                transfromComponent.Transform.rotation = Quaternion.Euler(0, transfromComponent.Transform.rotation.eulerAngles.y, 0);

                if(_playerPool.Value.Has(entity) && _movePool.Value.Has(entity))
                {
                    ref var moveComp = ref _movePool.Value.Get(entity);
                    moveComp.MoveDirection = moveComp.TargetPosition - transfromComponent.Transform.position;
                    var test = transfromComponent.Transform.InverseTransformDirection(moveComp.MoveDirection).normalized;
                    animatorComponent.Animator.SetFloat("Forward", test.z, 0.1f, Time.deltaTime);
                    animatorComponent.Animator.SetFloat("Turn", test.x, 0.1f, Time.deltaTime);
                }
            }
        }
    }
}
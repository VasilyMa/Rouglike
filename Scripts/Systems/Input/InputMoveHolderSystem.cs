using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class InputMoveHolderSystem : MainEcsSystem
    {
        readonly EcsFilterInject<Inc<PlayerComponent,InputHolder,TransformComponent>, Exc<DeadComponent,InActionComponent,HardControlComponent>> _filter = default;
        readonly EcsPoolInject<InputHolder> _inputHolderPool = default;
        readonly EcsPoolInject<MoveComponent> _movePool = default;
        readonly EcsPoolInject<TransformComponent> _transformPool = default;
        readonly EcsPoolInject<MoveAnimationState> _animationMovePool = default;

        public override MainEcsSystem Clone()
        {
            return new InputMoveHolderSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach (var entity in _filter.Value)
            {
                ref var inputHolder = ref _inputHolderPool.Value.Get(entity);
                ref var transformComp = ref _transformPool.Value.Get(entity);
                if (inputHolder.MoveDirection.magnitude > 0.1f)
                {
                    if (!_movePool.Value.Has(entity))
                    {
                        _movePool.Value.Add(entity);
                        _animationMovePool.Value.Add(entity);
                    }
                    ref var moveComp = ref _movePool.Value.Get(entity);
                    moveComp.TargetPosition = transformComp.Transform.position + inputHolder.MoveDirection;
                   
                }
            }
        }
    }
}
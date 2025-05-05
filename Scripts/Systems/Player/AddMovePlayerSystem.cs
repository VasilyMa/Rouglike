using Leopotam.EcsLite;
using UnityEngine;
using Leopotam.EcsLite.Di;

namespace Client
{
    sealed class AddMovePlayerSystem : MainEcsSystem
    {
        readonly EcsFilterInject<Inc<InputHolder>> _inputHolderFilter = default;
        readonly EcsPoolInject<InputComponent> _inputPool = default;
        readonly EcsPoolInject<InputHolder> _inputHolderPool = default;
        readonly EcsPoolInject<WASDInputEvent> _wasdPool = default;
        readonly EcsFilterInject<Inc<PlayerComponent, TransformComponent>, Exc<DeadComponent, LockMoveComponent, KnockbackComponent>> _filter = default;
        readonly EcsPoolInject<MoveComponent> _movePool = default;
        readonly EcsPoolInject<ExternalMoveComponent> _externalMovePool = default;
        readonly EcsPoolInject<TransformComponent> _transformPool = default;

        float _playerSpeed;
        const float PLAYER_MOVE_DURATION = 0;

        public override MainEcsSystem Clone()
        {
            return new AddMovePlayerSystem();
        }
        public override void Init(IEcsSystems systems)
        { _playerSpeed = ConfigModule.GetConfig<GameConfig>().PlayerSpeed; }
        public override void Run(IEcsSystems systems)
        {
            foreach (var player in _inputHolderFilter.Value)
            {
                ref var inputHolderComp = ref _inputHolderPool.Value.Get(player);
                foreach (var entity in _filter.Value)
                {
                    ref var transformComp = ref _transformPool.Value.Get(entity);

                    if (!_externalMovePool.Value.Has(entity))
                    {
                        _externalMovePool.Value.Add(entity).Invoke(inputHolderComp.MoveDirection, ForceMode.VelocityChange, _playerSpeed, PLAYER_MOVE_DURATION, isInterruptible: false);
                    }
                    else
                    {
                        ref var externalMoveComp = ref _externalMovePool.Value.Get(entity);
                        if (externalMoveComp.IsInterruptible)
                            externalMoveComp.Invoke((externalMoveComp.MoveDirection).normalized * externalMoveComp.Speed, ForceMode.VelocityChange, _playerSpeed, PLAYER_MOVE_DURATION, isInterruptible: false);
                    }

                    //ChangeAnimationController.ChangeAnimationFunc(AnimationTypes.Move, entity,rootMotion:false);
                }
            }
        }
    }
}
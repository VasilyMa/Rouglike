using Leopotam.EcsLite;
using UnityEngine;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class StopMoveSystem : MainEcsSystem 
    {
        readonly EcsSharedInject<GameState> _state = default;
        readonly EcsFilterInject<Inc<UnitComponent, PlayerComponent>, Exc<DeadComponent, MoveComponent, LockMoveComponent, WaitClick>>  _filter = default;
        readonly EcsPoolInject<ViewComponent> _viewPool = default;
        readonly EcsPoolInject<KnockbackComponent> _knockBackPool = default;
        readonly EcsPoolInject<AnimationStateComponent> _changeAnimationPool = default;
        readonly EcsPoolInject<MoveAbilityComponent> _movePool;
        readonly EcsPoolInject<IdleAnimationState> _idleAnimationPool = default;

        public override MainEcsSystem Clone()
        {
            return new StopMoveSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach(var entity in _filter.Value)
            {
                if(_knockBackPool.Value.Has(entity)) continue;

                _idleAnimationPool.Value.Add(entity);
            }
        }
    }
}
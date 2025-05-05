using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using UnityEngine;

namespace Client 
{
    sealed class MoveAnimationSystem : MainEcsSystem 
    {
        readonly EcsFilterInject<Inc<MoveAnimationState, AnimatorComponent>, Exc<IdleAnimationState, SpawnAnimationState>> _filter = default;
        readonly EcsPoolInject<AnimatorComponent> _animatorPool = default;
        readonly EcsPoolInject<MoveAnimationState> _movePool = default;

        public override MainEcsSystem Clone()
        {
            return new MoveAnimationSystem();
        }

        public override void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter.Value)
            {
                ref var animatorComp = ref _animatorPool.Value.Get(entity);
                ref var moveComp = ref _movePool.Value.Get(entity);
                animatorComp.Animator.applyRootMotion = moveComp.IsRootMotion;
                animatorComp.Animator.SetBool(AnimatorComponent.IsMove, true);
            }
        }
    }
}
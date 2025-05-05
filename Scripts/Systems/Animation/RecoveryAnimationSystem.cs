using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using UnityEngine;

namespace Client {
    sealed class RecoveryAnimationSystem : MainEcsSystem
    {
        readonly EcsFilterInject<Inc<RecoveryAnimationState, AnimatorComponent>, Exc<HardControlComponent>> _filter = default;
        readonly EcsPoolInject<AnimatorComponent> _animatorPool = default;
        readonly EcsPoolInject<RecoveryAnimationState> _recoveryAnimationPool = default;

        public override MainEcsSystem Clone()
        {
            return new RecoveryAnimationSystem();
        }

        public override void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter.Value)
            {
                ref var animatorComp = ref _animatorPool.Value.Get(entity);
                ref var recoveryAnimationComp = ref _recoveryAnimationPool.Value.Get(entity);

                animatorComp.Animator.applyRootMotion = recoveryAnimationComp.IsRootMotion;
                animatorComp.Animator.SetTrigger(AnimatorComponent.Recovery);
            }
        }
    }
}
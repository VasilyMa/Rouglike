using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using UnityEngine;

namespace Client 
{
    sealed class CastAnimationSystem : MainEcsSystem 
    {
        readonly EcsFilterInject<Inc<CastAnimationState, AnimatorComponent>, Exc<DeadComponent>> _filter = default;
        readonly EcsPoolInject<AnimatorComponent> _animatorPool = default;
        readonly EcsPoolInject<CastAnimationState> _castAnimationPool = default;

        public override MainEcsSystem Clone()
        {
            return new CastAnimationSystem();
        }

        public override void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter.Value)
            {
                ref var animatorComp = ref _animatorPool.Value.Get(entity);
                ref var castAnimationComp = ref _castAnimationPool.Value.Get(entity);

                animatorComp.Animator.applyRootMotion = castAnimationComp.IsRootMotion;

                switch (castAnimationComp.Type)
                {
                    case CastAnimationType.prepare:
                        animatorComp.Animator.SetTrigger(AnimatorComponent.Prepare);
                        
                        break;
                    case CastAnimationType.cast:
                        animatorComp.Animator.SetBool(AnimatorComponent.IsPrepare, false);
                        animatorComp.Animator.SetTrigger(AnimatorComponent.Cast);
                        
                        break;
                }
            }
        }
    }
}
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client 
{
    sealed class KnockbackAnimationSystem : MainEcsSystem 
    {
        readonly EcsFilterInject<Inc<KnockbackAnimationState, AnimatorComponent>, Exc<DeadComponent>> _filter = default;
        readonly EcsPoolInject<AnimatorComponent> _animatorPool = default;
        readonly EcsPoolInject<KnockbackAnimationState> _knockbackPool = default;

        public override MainEcsSystem Clone()
        {
            return new KnockbackAnimationSystem();
        }

        public override void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter.Value)
            {
                ref var animatorComp = ref _animatorPool.Value.Get(entity);
                ref var knockbackComp = ref _knockbackPool.Value.Get(entity);

                animatorComp.Animator.applyRootMotion = knockbackComp.IsRootMotion;

                switch (knockbackComp.KnockbackState)
                {
                    case KnockbackState.knockback: animatorComp.Animator.SetTrigger(AnimatorComponent.Knockback);
                        break;
                    case KnockbackState.getup: animatorComp.Animator.SetTrigger(AnimatorComponent.GetUp);
                        break;
                }

            }
        }
    }
}
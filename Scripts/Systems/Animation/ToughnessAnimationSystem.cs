using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client 
{
    sealed class ToughnessAnimationSystem : MainEcsSystem 
    {
        readonly EcsFilterInject<Inc<ToughnessAnimationState, AnimatorComponent>, Exc<DeadComponent>> _filter = default;
        readonly EcsPoolInject<AnimatorComponent> _animatorPool = default;
        readonly EcsPoolInject<ToughnessAnimationState> _tiredPool = default;

        public override MainEcsSystem Clone()
        {
            return new ToughnessAnimationSystem();
        }

        public override void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter.Value)
            {
                ref var animatorComp = ref _animatorPool.Value.Get(entity);
                ref var tiredComp = ref _tiredPool.Value.Get(entity);

                animatorComp.Animator.SetTrigger(AnimatorComponent.Tired);
                animatorComp.Animator.SetBool(AnimatorComponent.IsToughness, false);
                animatorComp.Animator.SetBool(AnimatorComponent.IsTired, true);
                animatorComp.Animator.applyRootMotion = tiredComp.IsRootMotion;
            }
        }
    }
}
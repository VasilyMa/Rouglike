using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class DashAnimationSystem : MainEcsSystem 
    {
        readonly EcsFilterInject<Inc<DashAnimationState, AnimatorComponent>, Exc<SpawnAnimationState>> _filter = default;
        readonly EcsPoolInject<AnimatorComponent> _animatorPool = default;
        readonly EcsPoolInject<DashAnimationState> _dashPool = default;

        public override MainEcsSystem Clone()
        {
            return new DashAnimationSystem();
        }

        public override void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter.Value)
            {
                ref var animatorComp = ref _animatorPool.Value.Get(entity);
                ref var dashComp = ref _dashPool.Value.Get(entity);
                animatorComp.Animator.applyRootMotion = dashComp.IsRootMotion;
                animatorComp.Animator.SetTrigger(AnimatorComponent.Dash);
                animatorComp.Animator.SetBool(AnimatorComponent.IsPrepare, false);
            }
        }
    }
}
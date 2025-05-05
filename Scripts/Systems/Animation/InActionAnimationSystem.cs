using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class InActionAnimationSystem : MainEcsSystem 
    {
        readonly EcsFilterInject<Inc<AnimatorComponent, InActionAnimationState>> _filter = default;
        readonly EcsPoolInject<AnimatorComponent> _animatorPool = default;
        readonly EcsPoolInject<InActionAnimationState> _inActionAnimationState = default;

        public override MainEcsSystem Clone()
        {
            return new InActionAnimationSystem();
        }

        public override void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter.Value)
            {
                ref var animatorComp = ref _animatorPool.Value.Get(entity);
                ref var actionComp = ref _inActionAnimationState.Value.Get(entity);

                animatorComp.Animator.SetBool(AnimatorComponent.IsInAction, false);
            }
        }
    }
}
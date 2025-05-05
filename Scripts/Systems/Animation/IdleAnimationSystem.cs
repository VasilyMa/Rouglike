using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class IdleAnimationSystem : MainEcsSystem
    {
        readonly EcsFilterInject<Inc<IdleAnimationState, AnimatorComponent>, Exc<SpawnAnimationState/*, InActionComponent*/>> _filter = default;
        readonly EcsPoolInject<AnimatorComponent> _animatorPool = default;
        readonly EcsPoolInject<IdleAnimationState> _idlePool = default;

        public override MainEcsSystem Clone()
        {
            return new IdleAnimationSystem();
        }

        public override void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref var animatorComp = ref _animatorPool.Value.Get(entity);
                ref var idleComp = ref _idlePool.Value.Get(entity);
                animatorComp.Animator.applyRootMotion = idleComp.IsRootMotion;
                animatorComp.Animator.SetBool(AnimatorComponent.IsMove, false);
            }
        }
    }
}
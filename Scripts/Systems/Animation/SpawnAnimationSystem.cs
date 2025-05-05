using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class SpawnAnimationSystem : MainEcsSystem 
    {
        readonly EcsFilterInject<Inc<SpawnAnimationState>, Exc<DeadComponent>> _filter = default;
        readonly EcsPoolInject<AnimatorComponent> _animatorPool = default;
        readonly EcsPoolInject<SpawnAnimationState> _spawnPool = default;

        public override MainEcsSystem Clone()
        {
            return new SpawnAnimationSystem();
        }

        public override void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter.Value)
            {
                ref var animatorComp = ref _animatorPool.Value.Get(entity);
                ref var spawnComp = ref _spawnPool.Value.Get(entity);
                animatorComp.Animator.applyRootMotion = spawnComp.IsRootMotion;
                animatorComp.Animator.SetTrigger(AnimatorComponent.Spawm);
            }
        }
    }
}
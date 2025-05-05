using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client 
{
    sealed class HitAnimationSystem : MainEcsSystem 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsFilterInject<Inc<HitAnimationState>, Exc<DeadComponent, SpawnAnimationState>> _filter = default;
        readonly EcsPoolInject<AnimatorComponent> _animatorPool = default;
        readonly EcsPoolInject<HitAnimationState> _hitAnimationPool = default;
        readonly EcsPoolInject<ShieldsContainer> _shieldPool = default;
        public override MainEcsSystem Clone()
        {
            return new HitAnimationSystem();
        }

        public override void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter.Value)
            {
                if (_shieldPool.Value.Has(entity))
                {
                    ref var shieldComp = ref _shieldPool.Value.Get(entity);

                    if (shieldComp.shieldComponents.Count > 0) continue;
                }

                ref var animatorComp = ref _animatorPool.Value.Get(entity);
                ref var hitComp = ref _hitAnimationPool.Value.Get(entity);

                animatorComp.Animator.applyRootMotion = hitComp.IsRootMotion;
                animatorComp.Animator.SetBool(AnimatorComponent.IsPrepare, false);
                switch (hitComp.Type)
                {
                    case HitAnimationType.GetHitFront:
                        animatorComp.Animator.SetTrigger(AnimatorComponent.HitFront);
                        break;
                    case HitAnimationType.GetHitRight:
                        animatorComp.Animator.SetTrigger(AnimatorComponent.HitRight);
                        break;
                    case HitAnimationType.GetHitLeft:
                        animatorComp.Animator.SetTrigger(AnimatorComponent.HitLeft);
                        break;
                    case HitAnimationType.GetHitBack:
                        animatorComp.Animator.SetTrigger(AnimatorComponent.HitBack);
                        break;
                }
            }
        }
    }
}
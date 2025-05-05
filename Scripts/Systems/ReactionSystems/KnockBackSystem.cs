using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    sealed class KnockBackSystem : MainEcsSystem
    {
        readonly EcsSharedInject<GameState> _state = default;
        readonly EcsWorldInject _world;
        private readonly EcsFilterInject<Inc<KnockbackEffect>,Exc<HighToughnessComponent,StaticUnitComponent>> _filter = default;
        private readonly EcsPoolInject<KnockbackEffect> _pool = default;
        readonly EcsPoolInject<RequestAddHardControlEvent> _requestHardControlPool = default;
        readonly EcsPoolInject<KnockbackAnimationState> _knockbackAnimationPool = default;
        readonly EcsPoolInject<AnimatorComponent> _animatorPool = default;


        public override MainEcsSystem Clone()
        {
            return new KnockBackSystem();
        }

        public override void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref var knockComp = ref _pool.Value.Get(entity);

                if (!knockComp.IsKnocked)
                {
                    if(!_knockbackAnimationPool.Value.Has(entity)) _knockbackAnimationPool.Value.Add(entity);
                    _knockbackAnimationPool.Value.Get(entity).KnockbackState = KnockbackState.knockback;
                    //ChangeAnimationController.ChangeAnimationFunc(AnimationTypes.KnockBack, entity);
                    knockComp.IsKnocked = true;
                    knockComp.IsGetUp = false;
                    ref var requestComp = ref _requestHardControlPool.Value.Add(_world.Value.NewEntity());
                    //todo SASHA CONTROL TIME
                    requestComp.TargetEntity = _world.Value.PackEntity(entity);
                    requestComp.ControlTime = knockComp.Duration;


                    ref var animatorComp = ref _animatorPool.Value.Get(entity);
                    var info = animatorComp.Animator.runtimeAnimatorController.animationClips;
                    
                    for (int i = 0; i < info.Length; i++)
                    {
                        if(info[i].name.Contains("getup")) knockComp.GetUpTimer = info[i].length;
                    }
                    if(knockComp.GetUpTimer >= knockComp.Duration) knockComp.GetUpTimer = knockComp.Duration - 0.1f;
                }
                
                if (!knockComp.IsGetUp && knockComp.Duration - knockComp.GetUpTimer <= 0)
                {
                    knockComp.IsGetUp = true;
                    if(!_knockbackAnimationPool.Value.Has(entity)) _knockbackAnimationPool.Value.Add(entity);
                    _knockbackAnimationPool.Value.Get(entity).KnockbackState = KnockbackState.getup;
                }
                knockComp.Duration -= Time.deltaTime;
                if(knockComp.Duration <= 0) _pool.Value.Del(entity);
            }
        }
    }
}
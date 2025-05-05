using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client {
    sealed class AddWaitClickTimerSystem : MainEcsSystem
    {
        readonly EcsWorldInject _world = default;
        readonly EcsFilterInject<Inc<DisposeAbilityEvent, OwnerComponent, TimerAbilityComponent>> _filter = default;
        readonly EcsPoolInject<AbilityComponent> _abilityPool = default;
        readonly EcsPoolInject<OwnerComponent> _ownerPool = default;
        readonly EcsPoolInject<AbilityUnitComponent> _unitMBPool = default;
        readonly EcsPoolInject<WaitClick> _waitClickPool = default;
        readonly EcsPoolInject<AnimatorComponent> _animationPool = default;
        readonly EcsPoolInject<NonWaitClickable> _nonWaitPool = default;
        readonly EcsPoolInject<StaticUnitComponent> _staticUnitPool;

        public override MainEcsSystem Clone()
        {
            return new AddWaitClickTimerSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach(var entity in _filter.Value)
            {
                ref var ownerComp = ref _ownerPool.Value.Get(entity);
                if(ownerComp.OwnerEntity.Unpack(_world.Value, out int ownerEntity))
                {
                    ref var abilityComp = ref _abilityPool.Value.Get(entity);
                    ref var unitMBComp = ref _unitMBPool.Value.Get(ownerEntity);
                    
                    unitMBComp.AbilityUnitMB.CurrentAbility = abilityComp.Ability.name;
                    if (!_nonWaitPool.Value.Has(ownerEntity)&& _animationPool.Value.Has(ownerEntity) && !_staticUnitPool.Value.Has(entity))
                    {
                        if (!_waitClickPool.Value.Has(ownerEntity)) _waitClickPool.Value.Add(ownerEntity);
                        ref var waitClickComp = ref _waitClickPool.Value.Get(ownerEntity);

                        ref var animationComp = ref _animationPool.Value.Get(ownerEntity);
                        AnimatorStateInfo stateInfo = animationComp.Animator.GetCurrentAnimatorStateInfo(0);
                        float timeRemaining = stateInfo.length * (1 - stateInfo.normalizedTime);
                        waitClickComp.InitComponent(timeRemaining);
                    }
                    else
                    {
                        _nonWaitPool.Value.Del(ownerEntity);
                    }
                   
                }
            }
        }
    }
}
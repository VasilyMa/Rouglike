using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client {
    sealed class RequestChangeAnimationEventSystem : MainEcsSystem 
    {  
        readonly EcsWorldInject _world = default;
        readonly EcsFilterInject<Inc<RequestChangeAnimationEvent>, Exc<AnimationStateComponent>> _filter = default;
        readonly EcsPoolInject<RequestChangeAnimationEvent> _requestPool = default;
        readonly EcsPoolInject<AnimationStateComponent> _changeAnimationPool = default;

        public override MainEcsSystem Clone()
        {
            return new RequestChangeAnimationEventSystem();
        }

        public override void Run (IEcsSystems systems) 
        {
            foreach(var entity in _filter.Value)
            {
                ref var requestComp = ref _requestPool.Value.Get(entity);
                if(requestComp.TargetPackedEntity.Unpack(_world.Value, out int targetEntity))
                {
                    if (!_changeAnimationPool.Value.Has(targetEntity))  _changeAnimationPool.Value.Add(targetEntity);
                    
                    ref var changeAnimationComp = ref _changeAnimationPool.Value.Get(targetEntity);
                    changeAnimationComp.AnimationType = requestComp.AnimationType;
                    changeAnimationComp.RootMotion = requestComp.RootMotion;
                    changeAnimationComp.UniqueAnimation = requestComp.UniqueAnimation;
                    changeAnimationComp.IsUniqueAnimation = requestComp.IsUniqueAnimation;


                }
            }
        }
    }
}
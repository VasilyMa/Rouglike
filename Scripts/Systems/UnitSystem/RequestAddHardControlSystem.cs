using Leopotam.EcsLite;
using UnityEngine;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class RequestAddHardControlSystem : MainEcsSystem 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsFilterInject<Inc<RequestAddHardControlEvent>> _filter = default;
        readonly EcsPoolInject<RequestAddHardControlEvent> _evtPool = default;
        readonly EcsPoolInject<AddHardControlEvent> _addHardControlPool = default;
        readonly EcsPoolInject<AnimatorComponent> _animatorPool = default;
        readonly EcsPoolInject<PlayerComponent> _playerPool = default;
        public override MainEcsSystem Clone()
        {
            return new RequestAddHardControlSystem();
        }
        public override void Run (IEcsSystems systems) {
            foreach(var entity in _filter.Value)
            {
                ref var evtComp = ref _evtPool.Value.Get(entity);
                if(evtComp.TargetEntity.Unpack(_world.Value, out int targetEntity))
                {
                    if(!_addHardControlPool.Value.Has(targetEntity)) _addHardControlPool.Value.Add(targetEntity);
                    ref var addHardControlComp = ref _addHardControlPool.Value.Get(targetEntity);
                    if(evtComp.TheTimerShouldBeSetToTheTimeUntilTheEndOfTheAnimation)
                    {
                        ref var animatorComp = ref _animatorPool.Value.Get(targetEntity);
                        var info = animatorComp.Animator.GetCurrentAnimatorStateInfo(0);
                        
                        evtComp.ControlTime = info.length;
                        if (_playerPool.Value.Has(targetEntity))
                        {
                            evtComp.ControlTime = 0.4f;
                        }
                    }

                    if(addHardControlComp.ControlTime < evtComp.ControlTime)
                    {
                        addHardControlComp.ControlTime = evtComp.ControlTime;
                    }
                   
                }
            }
        }
    }
}
using UnityEngine;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class AddHardControlSystem : MainEcsSystem 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsFilterInject<Inc<AddHardControlEvent>> _filter = default;
        readonly EcsPoolInject<AddHardControlEvent> _evtPool = default;
        readonly EcsPoolInject<HardControlComponent> _hardControlPool = default;
        readonly EcsPoolInject<HardControlTimerComponent> _timerPool = default;
        readonly EcsPoolInject<RequestLockMoveEvent> _requestLockMovePool = default;
        readonly EcsPoolInject<RequestLockRotationEvent> _requestLockRotationPool = default;

        public override MainEcsSystem Clone()
        {
            return new AddHardControlSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach(var entity in _filter.Value)
            {
                ref var evtComp = ref _evtPool.Value.Get(entity);
                if(!_hardControlPool.Value.Has(entity)) _hardControlPool.Value.Add(entity);
                if(!_timerPool.Value.Has(entity)) _timerPool.Value.Add(entity);
                //if(entity == GameState.Instance.PlayerEntity) Debug.Break();
                ref var timerComp = ref _timerPool.Value.Get(entity);
                if(timerComp.ControlTime < evtComp.ControlTime)
                {
                    timerComp.ControlTime = evtComp.ControlTime;
                }

                ref var lockMoveComp = ref _requestLockMovePool.Value.Add(_world.Value.NewEntity());
                lockMoveComp.TargetPackedEntity = _world.Value.PackEntity(entity);

                ref var lockRotationComp = ref _requestLockRotationPool.Value.Add(_world.Value.NewEntity());
                lockRotationComp.TargetPackedEntity = _world.Value.PackEntity(entity);

                
            }
        }
    }
}
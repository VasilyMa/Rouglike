using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client {
    sealed class HardControlTimerCheckSystem : MainEcsSystem 
    {
        readonly EcsFilterInject<Inc<HardControlTimerComponent, HardControlComponent>> _filter = default;
        readonly EcsPoolInject<HardControlTimerComponent> _timerPool = default;
        readonly EcsPoolInject<DelHardControlEvent> _delEvtPool = default;

        public override MainEcsSystem Clone()
        {
            return new HardControlTimerCheckSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach(var entity in _filter.Value)
            {
                ref var timerComp = ref _timerPool.Value.Get(entity);
                timerComp.ControlTime -= Time.deltaTime;
                if(timerComp.ControlTime <= 0)
                {
                    _delEvtPool.Value.Add(entity);
                }
            }
        }
    }
}
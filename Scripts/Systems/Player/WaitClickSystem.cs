using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using static SlowVisualConfig;

namespace Client {
    sealed class WaitClickTimerSystem : MainEcsSystem
    {
        readonly EcsFilterInject<Inc<WaitClick, PlayerComponent>> _filter = default;
        readonly EcsPoolInject<WaitClick> _waitPool = default;
        readonly EcsPoolInject<DelWaitClick> _delWaitClick = default;

        public override MainEcsSystem Clone()
        {
            return new WaitClickTimerSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach(var entity in _filter.Value)
            {
                ref var waitComp = ref _waitPool.Value.Get(entity);
                
                waitComp.CurrentTime += Time.deltaTime;
                if(waitComp.CurrentTime >= waitComp.TargetTime)
                {
                    //todo event resetCombo
                    _delWaitClick.Value.Add(entity);
                }
            }
        }
    }
}
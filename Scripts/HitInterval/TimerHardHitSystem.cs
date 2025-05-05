using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client {
    sealed class TimerHardHitSystem : MainEcsSystem {
        readonly EcsFilterInject<Inc<HardHitComponent>> _filter;
        readonly EcsPoolInject<HardHitComponent> _hardHitPool;
        public override MainEcsSystem Clone()
        {
            return new TimerHardHitSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach(var entity in _filter.Value)
            {
                ref var hardHitComp = ref _hardHitPool.Value.Get(entity);
                hardHitComp.TimerHardHit -= Time.deltaTime;
                if (hardHitComp.TimerHardHit > 0) continue;
                _hardHitPool.Value.Del(entity);
            }
        }
    }
}
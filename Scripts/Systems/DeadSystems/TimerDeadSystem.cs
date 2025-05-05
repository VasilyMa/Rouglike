using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using UI;

using UnityEngine;

namespace Client {
    sealed class TimerDeadSystem : MainEcsSystem 
    {
        readonly EcsFilterInject<Inc<DeadComponent>> _filter;
        readonly EcsPoolInject<DeadComponent> _deadComponent;

        readonly EcsPoolInject<DelEntityEvent> _delPool = default;

        public override MainEcsSystem Clone()
        {
            return new TimerDeadSystem();
        }

        public override void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter.Value) 
            {
                ref var deadComponent = ref _deadComponent.Value.Get(entity);
                deadComponent.TimerToDestroy+=Time.deltaTime;
                if (deadComponent.TimerToDestroy < deadComponent.TimeOfDeath) continue;
                _delPool.Value.Add(entity);
            }
        }
    }
}
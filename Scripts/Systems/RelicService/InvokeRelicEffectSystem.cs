using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using UnityEngine;

namespace Client {
    sealed class InvokeRelicEffectSystem : MainEcsSystem 
    {
        readonly EcsFilterInject<Inc<InvokeRelicEvent>> _filter = default;
        readonly EcsPoolInject<InvokeRelicEvent> _relicEventPool = default;

        public override MainEcsSystem Clone()
        {
            return new InvokeRelicEffectSystem();
        }

        public override void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter.Value)
            { 
                _relicEventPool.Value.Get(entity).Resolve();
            }
        }
    }
}
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    public class DelHere_InvokeVisualEffectEvent : MainEcsSystem
    {
        readonly EcsFilterInject<Inc<InvokeVisualEffectEvent>> _filter = default;
        
        public override MainEcsSystem Clone()
        {
            return new DelHere_InvokeVisualEffectEvent();
        }

        public override void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                _filter.Pools.Inc1.Del(entity);
            }
        }
    }
}
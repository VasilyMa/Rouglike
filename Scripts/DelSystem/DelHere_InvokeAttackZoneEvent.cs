using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    public class DelHere_InvokeAttackZoneEvent : MainEcsSystem
    {
        readonly EcsFilterInject<Inc<InvokeAttackZoneEvent>> _filter = default;
        
        public override MainEcsSystem Clone()
        {
            return new DelHere_InvokeAttackZoneEvent();
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
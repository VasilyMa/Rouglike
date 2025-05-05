using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    public class DelHereMaxPointConditionEvent: MainEcsSystem
    {
        readonly EcsFilterInject<Inc<MaxPointConditionEvent>> _filter = default;

        public override MainEcsSystem Clone()
        {
            return new DelHereMaxPointConditionEvent();
        }

        public override void Run(IEcsSystems systems)
        {
            foreach(var entity in _filter.Value)
            {
                _filter.Pools.Inc1.Del(entity);
            }
        }
    }
}

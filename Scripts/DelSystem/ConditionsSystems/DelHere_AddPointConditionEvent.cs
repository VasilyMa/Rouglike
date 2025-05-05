using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    public class DelHereAddPointConditionEvent: MainEcsSystem
    {
        readonly EcsFilterInject<Inc<AddPointConditionEvent>> _filter = default;

        public override MainEcsSystem Clone()
        {
            return new DelHereAddPointConditionEvent();
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

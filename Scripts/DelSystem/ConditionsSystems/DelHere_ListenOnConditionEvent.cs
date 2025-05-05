using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    public class DelHereListenOnConditionEvent: MainEcsSystem
    {
        readonly EcsFilterInject<Inc<ListenOnConditionEvent>> _filter = default;

        public override MainEcsSystem Clone()
        {
            return new DelHereListenOnConditionEvent();
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

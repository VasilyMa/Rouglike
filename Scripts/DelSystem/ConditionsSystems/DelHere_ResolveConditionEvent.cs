using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    public class DelHereResolveConditionEvent: MainEcsSystem
    {
        readonly EcsFilterInject<Inc<ResolveConditionEvent>> _filter = default;

        public override MainEcsSystem Clone()
        {
            return new DelHereResolveConditionEvent();
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

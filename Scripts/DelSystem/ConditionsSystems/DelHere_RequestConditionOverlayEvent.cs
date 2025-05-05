using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    public class DelHereRequestConditionOverlayEvent: MainEcsSystem
    {
        readonly EcsFilterInject<Inc<RequestConditionOverlayEvent>> _filter = default;

        public override MainEcsSystem Clone()
        {
            return new DelHereRequestConditionOverlayEvent();
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

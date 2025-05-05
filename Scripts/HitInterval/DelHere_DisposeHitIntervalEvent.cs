using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    public class DelHereDisposeHitIntervalEvent: MainEcsSystem
    {
        readonly EcsFilterInject<Inc<DisposeHitIntervalEvent>> _filter = default;

        public override MainEcsSystem Clone()
        {
            return new DelHereDisposeHitIntervalEvent();
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

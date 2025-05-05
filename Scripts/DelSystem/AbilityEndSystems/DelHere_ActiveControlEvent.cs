using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    public class DelHereActiveControlEvent: MainEcsSystem
    {
        readonly EcsFilterInject<Inc<ActiveControlEvent>> _filter = default;

        public override MainEcsSystem Clone()
        {
            return new DelHereActiveControlEvent();
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

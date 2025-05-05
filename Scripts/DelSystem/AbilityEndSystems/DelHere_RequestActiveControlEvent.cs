using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    public class DelHereRequestActiveControlEvent: MainEcsSystem
    {
        readonly EcsFilterInject<Inc<RequestActiveControlEvent>> _filter = default;

        public override MainEcsSystem Clone()
        {
            return new DelHereRequestActiveControlEvent();
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

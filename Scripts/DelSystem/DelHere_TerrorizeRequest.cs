using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    public class DelHereTerrorizeRequest: MainEcsSystem
    {
        readonly EcsFilterInject<Inc<TerrorizeRequest>> _filter = default;

        public override MainEcsSystem Clone()
        {
            return new DelHereTerrorizeRequest();
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
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    public class DelHereUnusedPerk: MainEcsSystem
    {
        readonly EcsFilterInject<Inc<UnusedPerk>> _filter = default;

        public override MainEcsSystem Clone()
        {
            return new DelHereUnusedPerk();
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

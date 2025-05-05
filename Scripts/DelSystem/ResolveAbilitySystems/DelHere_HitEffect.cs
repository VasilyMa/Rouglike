using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    public class DelHereHitEffect: MainEcsSystem
    {
        readonly EcsFilterInject<Inc<HitEffect>> _filter = default;

        public override MainEcsSystem Clone()
        {
            return new DelHereHitEffect();
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

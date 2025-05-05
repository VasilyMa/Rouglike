using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    public class DelHereRotationComponent: MainEcsSystem
    {
        readonly EcsFilterInject<Inc<RotationComponent>> _filter = default;

        public override MainEcsSystem Clone()
        {
            return new DelHereRotationComponent();
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
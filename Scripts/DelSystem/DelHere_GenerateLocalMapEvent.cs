using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    public class DelHere_GenerateLocalMapEvent : MainEcsSystem
    {
        readonly EcsFilterInject<Inc<GenerateLocalMapSelfRequest>> _filter = default;
        
        public override MainEcsSystem Clone()
        {
            return new DelHere_GenerateLocalMapEvent();
        }

        public override void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                _filter.Pools.Inc1.Del(entity);
            }
        }
    }
}
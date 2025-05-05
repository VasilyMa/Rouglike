using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    public class DelHere_CreateGlobalMapEvent : MainEcsSystem
    {
        readonly EcsFilterInject<Inc<CreateGlobalMapSelfRequest>> _filter = default;
        
        public override MainEcsSystem Clone()
        {
            return new DelHere_CreateGlobalMapEvent();
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
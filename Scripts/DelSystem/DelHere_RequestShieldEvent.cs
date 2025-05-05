using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    public class DelHere_RequestShieldEvent : MainEcsSystem
    {
        readonly EcsFilterInject<Inc<RequestShieldEvent>> _filter = default;
        
        public override MainEcsSystem Clone()
        {
            return new DelHere_RequestShieldEvent();
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
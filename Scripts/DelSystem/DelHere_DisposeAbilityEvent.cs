using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    public class DelHere_DisposeAbilityEvent : MainEcsSystem
    {
        readonly EcsFilterInject<Inc<DisposeAbilityEvent>> _filter = default;
        
        public override MainEcsSystem Clone()
        {
            return new DelHere_DisposeAbilityEvent();
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
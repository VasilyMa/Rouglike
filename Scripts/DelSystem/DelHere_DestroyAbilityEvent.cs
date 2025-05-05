using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    public class DelHere_DestroyAbilityEvent : MainEcsSystem
    {
        readonly EcsFilterInject<Inc<DestroyAbilityEvent>> _filter = default;
        
        public override MainEcsSystem Clone()
        {
            return new DelHere_DestroyAbilityEvent();
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
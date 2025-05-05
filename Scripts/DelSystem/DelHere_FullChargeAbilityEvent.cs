using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    public class DelHere_FullChargeAbilityEvent : MainEcsSystem
    {
        readonly EcsFilterInject<Inc<FullChargeAbilityEvent>> _filter = default;
        
        public override MainEcsSystem Clone()
        {
            return new DelHere_FullChargeAbilityEvent();
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
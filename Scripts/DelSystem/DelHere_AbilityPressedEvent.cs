using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    public class DelHere_AbilityPressedEvent : MainEcsSystem
    {
        readonly EcsFilterInject<Inc<AbilityPressedEvent>> _filter = default;
        
        public override MainEcsSystem Clone()
        {
            return new DelHere_AbilityPressedEvent();
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
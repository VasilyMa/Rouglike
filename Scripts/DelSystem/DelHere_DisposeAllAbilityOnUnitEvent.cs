using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    public class DelHere_DisposeAllAbilityOnUnitEvent : MainEcsSystem
    {
        readonly EcsFilterInject<Inc<DisposeAllAbilityOnUnitEvent>> _filter = default;
        
        public override MainEcsSystem Clone()
        {
            return new DelHere_DisposeAllAbilityOnUnitEvent();
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
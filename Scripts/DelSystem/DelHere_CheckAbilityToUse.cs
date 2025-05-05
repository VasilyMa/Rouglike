using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    public class DelHere_CheckAbilityToUse : MainEcsSystem
    {
        readonly EcsFilterInject<Inc<CheckAbilityToUse>> _filter = default;
        
        public override MainEcsSystem Clone()
        {
            return new DelHere_CheckAbilityToUse();
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
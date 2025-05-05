using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    public class DelHereAddAbilityEffect: MainEcsSystem
    {
        readonly EcsFilterInject<Inc<AddAbilityEffect>> _filter = default;

        public override MainEcsSystem Clone()
        {
            return new DelHereAddAbilityEffect();
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

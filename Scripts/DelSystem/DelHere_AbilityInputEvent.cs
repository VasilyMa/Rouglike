using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    public class DelHereAbilityInputEvent: MainEcsSystem
    {
        readonly EcsFilterInject<Inc<AbilityInputEvent>> _filter = default;

        public override MainEcsSystem Clone()
        {
            return new DelHereAbilityInputEvent();
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

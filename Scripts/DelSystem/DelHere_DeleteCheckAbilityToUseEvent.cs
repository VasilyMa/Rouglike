using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    public class DelHere_DeleteCheckAbilityToUseEvent : MainEcsSystem
    {
        readonly EcsFilterInject<Inc<DeleteCheckAbilityToUseEvent>> _filter = default;
        
        public override MainEcsSystem Clone()
        {
            return new DelHere_DeleteCheckAbilityToUseEvent();
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
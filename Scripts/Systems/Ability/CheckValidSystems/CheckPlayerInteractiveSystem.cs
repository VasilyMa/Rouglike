using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    public class CheckPlayerInteractiveSystem : MainEcsSystem
    {
        readonly EcsFilterInject<Inc<PlayerComponent,InteractWithObjectComponent>> _filter = default;
        readonly EcsFilterInject<Inc<CheckAbilityToUse>, Exc<ChargePointComponent, DeleteCheckAbilityToUseEvent>> _abilityfilter = default;
        readonly EcsPoolInject<CheckAbilityToUse> _checkPool = default;
        readonly EcsPoolInject<DeleteCheckAbilityToUseEvent> _delCheckPool = default;
        public override MainEcsSystem Clone()
        {
            return new CheckPlayerInteractiveSystem();
        }
        public override void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                foreach (var abilityEntity in _abilityfilter.Value)
                {
                    _delCheckPool.Value.Add(abilityEntity);
                }
            }
        }
    }
}
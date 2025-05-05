using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    public class StartCoolDownPlayerAbilitySystem : MainEcsSystem
    {
        readonly EcsFilterInject<Inc<StartCooldownAbilityEvent, AbilityComponent,CoolDownComponent, PlayerAbilityComponent>,Exc<CooldownRecalculationComponent>> _filter = default;
        readonly EcsPoolInject<StartCooldownAbilityEvent> _startCooldownPool = default;
        readonly EcsPoolInject<CoolDownComponent> _coolDownPool = default;
        readonly EcsPoolInject<CooldownRecalculationComponent> _coolDownRecalculationPool = default;
        public override MainEcsSystem Clone()
        {
            return new StartCoolDownPlayerAbilitySystem();
        }

        
        public override void Run(IEcsSystems systems)
        {
            foreach(var entity in _filter.Value)
            {
                _coolDownRecalculationPool.Value.Add(entity);
                ref var startCoolDownComp = ref _startCooldownPool.Value.Get(entity);

                ref var coolDownComp = ref _coolDownPool.Value.Get(entity);
                coolDownComp.CurrentCoolDownValue = startCoolDownComp.NormalCoolDown ? coolDownComp.CoolDownValue : 2f;

                //_startCooldownPool.Value.Del(entity);
            }
        }
    }
}
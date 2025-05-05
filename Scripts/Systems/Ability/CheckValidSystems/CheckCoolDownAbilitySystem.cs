using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class CheckCoolDownAbilitySystem : MainEcsSystem 
    {
        readonly EcsFilterInject<Inc<CoolDownComponent, CheckAbilityToUse>, Exc<ChargePointComponent, DeleteCheckAbilityToUseEvent>> _filter = default;
        readonly EcsPoolInject<CoolDownComponent> _coolDownPool = default;
        readonly EcsPoolInject<DeleteCheckAbilityToUseEvent> _deleteCheckPool = default;

        public override MainEcsSystem Clone()
        {
            return new CheckCoolDownAbilitySystem();
        }

        public override void Run (IEcsSystems systems) 
        {
            foreach(var entity in _filter.Value)
            {
                ref var coolDownComp = ref _coolDownPool.Value.Get(entity);
                if(coolDownComp.CurrentCoolDownValue > 0)
                {
                    _deleteCheckPool.Value.Add(entity);
                }
            }
        }
    }
}
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client 
{
    sealed class EndChargeAbilitySystem : MainEcsSystem 
    {
        readonly EcsFilterInject<Inc<EndChargeAbilityEvent, ChargeComponent, ChargeRecalculateComponent>> _filter = default;
        readonly EcsPoolInject<ChargeRecalculateComponent> _recalculatePool = default;

        public override MainEcsSystem Clone()
        {
            return new EndChargeAbilitySystem();
        }

        public override void Run (IEcsSystems systems) 
        {
            foreach(var entity in _filter.Value)
            {
                _recalculatePool.Value.Del(entity);
            }
        }
    }
}
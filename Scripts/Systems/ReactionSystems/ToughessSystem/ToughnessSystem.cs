using AbilitySystem;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    sealed class ToughnessSystem : MainEcsSystem
    {
        readonly EcsWorldInject _world = default;
        private readonly EcsFilterInject<Inc<TakeDamageComponent, ToughnessDamageAllowedComponent>> _filter = default;
        readonly EcsPoolInject<ToughnessComponent> _toughnessPool = default;
        private readonly EcsPoolInject<TakeDamageComponent> _takeDamagePool = default;
        readonly EcsPoolInject<RemoveHighToughnessEvent> _removeHighToughnessEvent;

        public override MainEcsSystem Clone()
        {
            return new ToughnessSystem();
        }

        public override void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {       
                ref var takeDamageComp = ref _takeDamagePool.Value.Get(entity);
                if(takeDamageComp.TargetEntity.Unpack(_world.Value, out int targetEntity))
                {
                    ref var toughnessComp = ref _toughnessPool.Value.Get(targetEntity); 
                    toughnessComp.CurrentValue -= takeDamageComp.Damage;
                    if (toughnessComp.CurrentValue > 0) continue;
                    toughnessComp.CurrentValue = 0;
                    if(!_removeHighToughnessEvent.Value.Has(targetEntity))_removeHighToughnessEvent.Value.Add(targetEntity);
                }
            }
        }
    }
}
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    public class AddHitIntervalSystem : MainEcsSystem
    {
        readonly EcsFilterInject<Inc<AddHitIntervalEvent>> _filter;
        readonly EcsPoolInject<AddHitIntervalEvent> _addHitIntervalPool;
        readonly EcsPoolInject<CalculationHitIntervalEvent> _calculationHitIntervalPool;
        readonly EcsPoolInject<DisposeHitIntervalEvent> _disposeHitIntervalPool;
        readonly EcsWorldInject _world;
        public override MainEcsSystem Clone()
        {
            return new AddHitIntervalSystem();
        }
        public override void Run(IEcsSystems systems)
        {
            foreach(var entity in _filter.Value)
            {
                ref var addHitIntervalComp = ref _addHitIntervalPool.Value.Get(entity);
                if (!addHitIntervalComp.TargetEntity.Unpack(_world.Value, out int targetEntity)) continue;
                if (!_calculationHitIntervalPool.Value.Has(targetEntity)) _calculationHitIntervalPool.Value.Add(targetEntity);
                ref var calculationHitIntervalComp = ref _calculationHitIntervalPool.Value.Get(targetEntity);
                calculationHitIntervalComp.Type = addHitIntervalComp.Type;
                if (!_disposeHitIntervalPool.Value.Has(targetEntity)) _disposeHitIntervalPool.Value.Add(targetEntity);
            }
        }
    }
}
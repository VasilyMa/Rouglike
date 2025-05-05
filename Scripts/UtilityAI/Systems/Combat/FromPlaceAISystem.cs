using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
namespace Client {
    sealed class FromPlaceAISystem : MainEcsSystem 
    {
        readonly EcsFilterInject<Inc<UnitBrain, TransformComponent, FromPlaceContext>,Exc<InActionComponent,HardControlComponent>> _filter = default;
        readonly EcsPoolInject<UnitBrain> _unitBrainPool = default;
        readonly EcsPoolInject<FromPlaceContext> _fromPlaceContextPool = default;
        readonly EcsPoolInject<TransformComponent> _transformPool = default;
        readonly EcsPoolInject<AbilityPressedEvent> _abilityPressedPool = default;
        readonly EcsWorldInject _world = default;

        public override MainEcsSystem Clone()
        {
            return new FromPlaceAISystem();
        }

        public override void Run(IEcsSystems systems)
        {
            foreach (int entity in _filter.Value)
            {
                ref var unitBrain = ref _unitBrainPool.Value.Get(entity);
                ref var fromPlaceContext = ref _fromPlaceContextPool.Value.Get(entity);
                ref var transformComp = ref _transformPool.Value.Get(entity);
                if (!fromPlaceContext.AnyActionUsable)
                {
                    continue;
                }
                if (Vector3.Distance(transformComp.Transform.position, fromPlaceContext.fromPlaceTransforms[0].transform.position) < 1)
                {
                    if (fromPlaceContext.validAbilitiesList[0].Unpack(_world.Value, out int abilityEntity))
                    {
                        _abilityPressedPool.Value.Add(abilityEntity);
                    }
                }
            }
        }
    }
}
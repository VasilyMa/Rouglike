using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class TerrorizeAISystem : MainEcsSystem
    {
        readonly EcsFilterInject<Inc<UnitBrain, TerrorizeTag, TransformComponent, EvaluationHelpersData>> _filter = default;
        readonly EcsPoolInject<UnitBrain> _unitBrainPool = default;
        readonly EcsPoolInject<TerrorizeTag> _terrorizeTagPool = default;
        readonly EcsPoolInject<AbilityPressedEvent> _abilityPressedPool = default;
        readonly EcsWorldInject _world = default;

        public override MainEcsSystem Clone()
        {
            return new TerrorizeAISystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach (int entity in _filter.Value)
            {
                ref var unitBrain = ref _unitBrainPool.Value.Get(entity);
                if (unitBrain.CurrentState != AIState.Terrorize)
                {
                    _terrorizeTagPool.Value.Del(entity);
                    continue;
                }
                if (unitBrain.bestTerrorizeActionAvailable.Unpack(_world.Value, out int bestAbilityEntity))
                {
                    _abilityPressedPool.Value.Add(bestAbilityEntity);
                }

            }
        }
    }
}
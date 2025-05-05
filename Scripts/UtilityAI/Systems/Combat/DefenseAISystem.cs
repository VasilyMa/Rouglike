using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
namespace Client
{
    sealed class DefenseAISystem : MainEcsSystem
    {
        readonly EcsFilterInject<Inc<UnitBrain, DefenseTag, TransformComponent, EvaluationHelpersData,ThreatsContext>> _filter = default;
        readonly EcsPoolInject<EvaluationHelpersData> _dataPool = default;
        readonly EcsPoolInject<TransformComponent> _transformPool = default;
        readonly EcsPoolInject<UnitBrain> _unitBrainPool = default;
        readonly EcsPoolInject<DefenseTag> _defenseTagPool = default;
        readonly EcsPoolInject<TargetsContext> _targetPool = default;
        readonly EcsPoolInject<ThreatsContext> _threatsPool = default;
        readonly EcsPoolInject<AbilityPressedEvent> _abilityPressedPool = default;
        readonly EcsPoolInject<AbilityUnitComponent> _abilityPool = default;
        readonly EcsWorldInject _world = default;

        public override MainEcsSystem Clone()
        {
            return new DefenseAISystem();
        }

        public override void Run(IEcsSystems systems)
        {
            foreach (int entity in _filter.Value)
            {
                ref var unitBrain = ref _unitBrainPool.Value.Get(entity);
                if (unitBrain.CurrentState != AIState.Defend)
                {
                    _defenseTagPool.Value.Del(entity);
                    continue;
                }
                ref var transformComponent = ref _transformPool.Value.Get(entity);
                ref var targetContext = ref _targetPool.Value.Get(entity);
                ref var data = ref _dataPool.Value.Get(entity);
                ref var threatsContext = ref _threatsPool.Value.Get(entity);
               // unitBrain.priorityPointToMove = transformComponent.Transform.position + threatsContext.incomingDirection;
                //unitBrain.priorityPointToLook = transformComponent.Transform.position + threatsContext.incomingDirection;

                if (unitBrain.bestDefensiveActionAvailable.Unpack(_world.Value, out int bestAbilityEntity))
                {
                    _abilityPressedPool.Value.Add(bestAbilityEntity); 

                }

            }
        }

    }
}
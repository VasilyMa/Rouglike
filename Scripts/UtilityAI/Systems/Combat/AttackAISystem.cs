using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
namespace Client
{
    sealed class AttackAISystem : MainEcsSystem
    {
        readonly EcsFilterInject<Inc<UnitBrain, AttackTag, TransformComponent, EvaluationHelpersData>,Exc<CoerciveAbilityComponent>> _filter = default;
        readonly EcsPoolInject<EvaluationHelpersData> _dataPool = default;
        readonly EcsPoolInject<TransformComponent> _transformPool = default;
        readonly EcsPoolInject<UnitBrain> _unitBrainPool = default;
        readonly EcsPoolInject<AttackTag> _attackTagPool = default;
        readonly EcsPoolInject<TargetsContext> _targetPool = default;
        readonly EcsPoolInject<AbilityPressedEvent> _abilityPressedPool = default;
        readonly EcsPoolInject<AbilityUnitComponent> _abilityPool = default;
        readonly EcsWorldInject _world = default;

        public override MainEcsSystem Clone()
        {
            return new AttackAISystem();
        }

        public override void Run(IEcsSystems systems)
        {
            foreach (int entity in _filter.Value)
            {
                ref var unitBrain = ref _unitBrainPool.Value.Get(entity);
                if (unitBrain.CurrentState != AIState.Attack)
                {
                    _attackTagPool.Value.Del(entity);
                    continue;
                }
                ref var attackTag = ref _attackTagPool.Value.Get(entity);
                ref var transformComponent = ref _transformPool.Value.Get(entity);
                ref var targetContext = ref _targetPool.Value.Get(entity);
                ref var data = ref _dataPool.Value.Get(entity);
                if (unitBrain.bestAttackAvailable.Unpack(_world.Value, out int bestAbilityEntity))
                {
                    _abilityPressedPool.Value.Add(bestAbilityEntity);
                }

            }
        }

    }
}
using Leopotam.EcsLite;
using UnityEngine;
using Leopotam.EcsLite.Di;


namespace Client
{
    sealed class ShelterEnemyLogicSystem : IEcsRunSystem
    {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<GameState> _state = default;
        readonly EcsFilterInject<Inc<TargetComponent, ShelterComponent>, Exc<DeadComponent, InActionComponent, HardControlComponent>> _filter = default;
        readonly EcsPoolInject<TargetComponent> _targetPool = default;
        readonly EcsPoolInject<EnemyComponent> _enemyPool = default;
        readonly EcsPoolInject<TransformComponent> _transformPool = default;
        readonly EcsPoolInject<SpecAttackButtonDownEvent> _specAttackButtonDownPool = default;
        readonly EcsPoolInject<AbilityUnitComponent> _abilityPool = default;
        readonly EcsPoolInject<AbilityPressedEvent> _abilityPressedPool = default;
        public void Run(IEcsSystems systems)
        {
            /*foreach (var entity in _filter.Value)
            {
                ref var targetComp = ref _targetPool.Value.Get(entity);
                if (targetComp.TargetPackedEntity.Unpack(_state.Value.World, out int targetEntity))
                {
                    ref var abilityComp = ref _abilityPool.Value.Get(entity);
                    ref var enemyComp = ref _enemyPool.Value.Get(entity);
                    ref var AIComp = ref _AIPool.Value.Get(entity);
                    ref var viewComp = ref _transformPool.Value.Get(entity);
                    ref var targetViewComp = ref _transformPool.Value.Get(targetEntity);
                    var distance = Vector3.Distance(viewComp.Transform.position, targetViewComp.Transform.position);

                    if (distance <= AIComp.MainAttackDistance)
                    {
                        var abilitiesList = abilityComp.AbilityUnitMB.GetAbilitiesListByActionNameTemp("Attack");
                        foreach (var abilityEntity in abilitiesList)
                        {
                            _abilityPressedPool.Value.Add(abilityEntity);
                        }
                    }

                    if (distance < AIComp.SecondaryAttackDistance)
                    {
                        var abilitiesList = abilityComp.AbilityUnitMB.GetAbilitiesListByActionNameTemp("SuperAttack");
                        foreach (var abilityEntity in abilitiesList)
                        {
                            _abilityPressedPool.Value.Add(abilityEntity);
                        }
                    }
                }
            }*/
        }
    }
}
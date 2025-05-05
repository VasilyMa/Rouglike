using Leopotam.EcsLite;
using UnityEngine;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class PursuerLogicSystem : IEcsRunSystem {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<GameState> _state = default;
        readonly EcsFilterInject<Inc<TargetComponent, PursuerComponent>, Exc<DeadComponent, InActionComponent, Circling, HardControlComponent>> _filter = default;
        readonly EcsPoolInject<TargetComponent> _targetPool = default;
        readonly EcsPoolInject<EnemyComponent> _enemyPool = default;
        readonly EcsPoolInject<TransformComponent> _transfromPool = default;
        readonly EcsPoolInject<NavMeshComponent> _navmeshPool = default;
        readonly EcsPoolInject<AbilityUnitComponent> _abilityPool = default;
        readonly EcsPoolInject<AbilityPressedEvent> _abilityPressedPool = default;
        public void Run (IEcsSystems systems) {
            /*foreach(var entity in _filter.Value)
            {
                ref var targetComp = ref _targetPool.Value.Get(entity);
                if(targetComp.TargetPackedEntity.Unpack(_state.Value.World, out int targetEntity))
                {
                    ref var enemyComp = ref _enemyPool.Value.Get(entity);
                    ref var AIComp = ref _AIPool.Value.Get(entity);
                    ref var navmeshComp = ref _navmeshPool.Value.Get(entity);
                    ref var transfromComp = ref _transfromPool.Value.Get(entity);
                    ref var targetTransfromComp = ref _transfromPool.Value.Get(targetEntity);
                    ref var abilityComp = ref _abilityPool.Value.Get(entity);

                    var distance = Vector3.Distance(transfromComp.Transform.position, targetTransfromComp.Transform.position);
                    if (distance <= AIComp.MainAttackDistance)
                    {
                        var abilitiesList = abilityComp.AbilityUnitMB.GetAbilitiesListByActionNameTemp("Attack");
                        foreach (var abilityEntity in abilitiesList)
                        {
                            _abilityPressedPool.Value.Add(abilityEntity);
                        }
                        

                    }
                    
                    if (distance <= AIComp.SecondaryAttackDistance)
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
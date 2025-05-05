using Leopotam.EcsLite;
using UnityEngine;
using Leopotam.EcsLite.Di;

namespace Client
{
    sealed class AddMoveShelterSystem : MainEcsSystem
    {
        readonly EcsSharedInject<GameState> _state = default;
        readonly EcsFilterInject<Inc<EnemyComponent, TargetComponent, ShelterComponent>, Exc<LockMoveComponent, HardControlComponent, Circling>> _filter = default;
        readonly EcsPoolInject<UnitComponent> _unitPool = default;
        readonly EcsPoolInject<MoveComponent> _movePool = default;
        readonly EcsPoolInject<NavMeshComponent> _navMeshPool = default;
        readonly EcsPoolInject<TargetComponent> _targetPool = default;
        readonly EcsPoolInject<EnemyComponent> _enemyPool = default;
        readonly EcsPoolInject<DeadComponent> _deadPool = default;
        readonly EcsPoolInject<TransformComponent> _transfromPool = default;

        public override MainEcsSystem Clone()
        {
            return new AddMoveShelterSystem();
        }

        public override void Run(IEcsSystems systems)
        {
            /*foreach (var entity in _filter.Value)
            {
                ref var navMeshComp = ref _navMeshPool.Value.Get(entity);
                ref var unitComp = ref _unitPool.Value.Get(entity);

                if (_deadPool.Value.Has(entity)) continue;
                ref var targetComp = ref _targetPool.Value.Get(entity);
                if (targetComp.TargetPackedEntity.Unpack(_state.Value.World, out int targetEntity))
                {
                    ref var enemyComp = ref _enemyPool.Value.Get(entity);
                    ref var targetViewComp = ref _transfromPool.Value.Get(targetEntity);
                    ref var transfromComp = ref _transfromPool.Value.Get(entity);
                    ref var AIComp = ref _AICompPool.Value.Get(entity);

                    var direction = (targetViewComp.Transform.position - transfromComp.Transform.position).normalized;
                    var distance = Vector3.Distance(transfromComp.Transform.position, targetViewComp.Transform.position);


                    if (distance <= AIComp.AvoidDistance && distance > AIComp.AvoidDistanceMin)
                    {
                        ref var moveComp = ref _movePool.Value.Add(entity);
                        moveComp.TargetPosition = transfromComp.Transform.position - direction * AIComp.AvoidDistance;
                        ChangeAnimation(entity, move: true);
                    }
                    else if (distance <= AIComp.AvoidDistance && distance < AIComp.AvoidDistanceMin)
                    {
                        ref var moveComp = ref _movePool.Value.Add(entity);
                        if (AIComp.changeDirectionDelay > 0)
                        {
                            AIComp.changeDirectionDelay -= Time.deltaTime;
                        }
                        else
                        {
                            moveComp.TargetPosition = Random.insideUnitSphere * 3; // sanya prosti chto nasral
                            ChangeAnimation(entity, move: true);
                            AIComp.changeDirectionDelay = 0.5f;
                        }
                    }
                    else if (distance <= AIComp.AgroDistance && distance > AIComp.AvoidDistance && distance > AIComp.MainAttackDistance)
                    {
                        ref var moveComp = ref _movePool.Value.Add(entity);
                        moveComp.TargetPosition = targetViewComp.Transform.position;

                        ChangeAnimation(entity, move: true);
                    }
                    else
                    {
                        *//* if (navMeshComp.NavMeshAgent.isOnNavMesh) navMeshComp.NavMeshAgent.isStopped = true;
                         navMeshComp.NavMeshAgent.SetDestination(transfromComp.Transform.position);

                         ChangeAnimation(entity, move : false);*//*
                        ChangeAnimationController.ChangeAnimationFunc(AnimationTypes.Idle, entity);
                    }
                }

                else
                {
                    ChangeAnimationController.ChangeAnimationFunc(AnimationTypes.Idle, entity);
                }

            }*/
        }
    }
}
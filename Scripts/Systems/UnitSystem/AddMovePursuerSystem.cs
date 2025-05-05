using Leopotam.EcsLite;
using UnityEngine;
using Leopotam.EcsLite.Di;

namespace Client
{
    sealed class AddMovePursuerSystem : MainEcsSystem
    {
        readonly EcsSharedInject<GameState> _state = default;
        readonly EcsFilterInject<Inc<EnemyComponent, TargetComponent, PursuerComponent>, Exc<LockMoveComponent, StunComponent, Circling>> _filter = default;
        readonly EcsPoolInject<UnitComponent> _unitPool = default;
        readonly EcsPoolInject<MoveComponent> _movePool = default;
        readonly EcsPoolInject<NavMeshComponent> _NavMeshPool = default;
        readonly EcsPoolInject<TransformComponent> _transfromPool = default;
        readonly EcsPoolInject<TargetComponent> _targetPool = default;
        readonly EcsPoolInject<DeadComponent> _deadPool = default;
        readonly EcsPoolInject<Circling> _circlingPool = default;

        public override MainEcsSystem Clone()
        {
            return new AddMovePursuerSystem();
        }

        public override void Run(IEcsSystems systems)
        {
            /*foreach (var entity in _filter.Value)
            {
                if (_deadPool.Value.Has(entity)) continue;
                ref var NavMeshComp = ref _NavMeshPool.Value.Get(entity);
                ref var unitComp = ref _unitPool.Value.Get(entity);
                ref var targetComp = ref _targetPool.Value.Get(entity);
                if (targetComp.TargetPackedEntity.Unpack(_state.Value.World, out int targetEntity))
                {
                    ref var AIComp = ref _AIPool.Value.Get(entity);
                    ref var targetViewComp = ref _transfromPool.Value.Get(targetEntity);
                    ref var transfromComp = ref _transfromPool.Value.Get(entity);
                    //                    ref var abilityComp = ref _abilityPool.Value.Get(entity);
                    //todo checkCoolDown ability
                    // if (abilityComp.GetAbility(AbilityTypes.Main).IsCooldown && abilityComp.GetAbility(AbilityTypes.Secondary).IsCooldown) // agra dlya triggera na igroka a ne main atack
                    // {
                    //     _circlingPool.Value.Add(entity);
                    //     NavMeshComp.NavMeshAgent.speed = 2f;
                    //     continue;
                    // }
                    ref var moveComp = ref _movePool.Value.Add(entity);
                    moveComp.TargetPosition = targetViewComp.Transform.position;
                    ChangeAnimationController.ChangeAnimationFunc(AnimationTypes.Move, entity,rootMotion:false);
                }
                else
                {
                    ChangeAnimationController.ChangeAnimationFunc(AnimationTypes.Idle, entity,rootMotion: false);
                }

            }*/
        }   
    }
}
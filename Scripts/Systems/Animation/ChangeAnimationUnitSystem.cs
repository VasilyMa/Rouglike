using Leopotam.EcsLite;
using UnityEngine;
using Leopotam.EcsLite.Di;
using System.Collections.Generic;
using UnityEngine.Animations;

namespace Client {
    sealed class ChangeAnimationUnitSystem : MainEcsSystem {
        readonly EcsWorldInject _world = default;
        readonly EcsFilterInject<Inc<AnimationStateComponent>> _filter = default;
        readonly EcsPoolInject<AnimationStateComponent> _changeAnimationPool = default;
        readonly EcsPoolInject<AbilityUnitComponent> _abilityUnitPool = default;
        readonly EcsPoolInject<AttackAnimationState> _attackAnimationPool = default;
        readonly EcsPoolInject<DashAnimationState> _dashAnimationPool = default;
        readonly EcsPoolInject<PlayerComponent> _playerPool = default;
        readonly EcsPoolInject<CastAnimationState> _castPool = default;
        readonly EcsPoolInject<SpawnAnimationState> _spawnPool = default;
        readonly EcsPoolInject<ToughnessAnimationState> _toughnessAnimationPool = default;
        readonly EcsPoolInject<RecoveryAnimationState> _recoveryAnimationPool = default;
        public override MainEcsSystem Clone()
        {
            return new ChangeAnimationUnitSystem();
        }

        public override void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter.Value)
            {
                ref var animationComp = ref _changeAnimationPool.Value.Get(entity);
                ref var abilityUnitComp = ref _abilityUnitPool.Value.Get(entity);

                switch (animationComp.AnimationType)
                {
                    case AnimationTypes.Attack:
                        if (!_attackAnimationPool.Value.Has(entity))
                        {
                            ref var animationAttack1Comp = ref _attackAnimationPool.Value.Add(entity);
                            animationAttack1Comp.AttackAnimationType = AttackAnimationType.attack1;
                            animationAttack1Comp.IsRootMotion = animationComp.RootMotion;
                        }
                        break;
                    case AnimationTypes.Attack2:
                        if (!_attackAnimationPool.Value.Has(entity))
                        {
                            ref var animationAttack2Comp = ref _attackAnimationPool.Value.Add(entity);
                            animationAttack2Comp.AttackAnimationType = AttackAnimationType.attack2;
                            animationAttack2Comp.IsRootMotion = animationComp.RootMotion;
                        }
                        break;
                    case AnimationTypes.Attack3:
                        if (!_attackAnimationPool.Value.Has(entity))
                        {
                            ref var animationAttack3Comp = ref _attackAnimationPool.Value.Add(entity);
                            animationAttack3Comp.AttackAnimationType = AttackAnimationType.attack3;
                            animationAttack3Comp.IsRootMotion = animationComp.RootMotion;
                        }
                        break;
                    case AnimationTypes.SpecAttack:
                        if (!_attackAnimationPool.Value.Has(entity))
                        {
                            ref var animationAttack4Comp = ref _attackAnimationPool.Value.Add(entity);
                            animationAttack4Comp.AttackAnimationType = AttackAnimationType.specAttack;
                            animationAttack4Comp.IsRootMotion = animationComp.RootMotion;
                        }
                        break;
                    case AnimationTypes.SpecAttack2:
                        if (!_attackAnimationPool.Value.Has(entity))
                        {
                            ref var animationAttack5Comp = ref _attackAnimationPool.Value.Add(entity);
                            animationAttack5Comp.AttackAnimationType = AttackAnimationType.specAttack2;
                            animationAttack5Comp.IsRootMotion = animationComp.RootMotion;
                        }
                        break;
                    case AnimationTypes.SpecAttack3:
                        if (!_attackAnimationPool.Value.Has(entity))
                        {
                            ref var animationAttack6Comp = ref _attackAnimationPool.Value.Add(entity);
                            animationAttack6Comp.AttackAnimationType = AttackAnimationType.specAttack3;
                            animationAttack6Comp.IsRootMotion = animationComp.RootMotion;
                        }
                        break;
                    case AnimationTypes.Dash:
                        if (!_dashAnimationPool.Value.Has(entity))
                        {
                            ref var dashComp = ref _dashAnimationPool.Value.Add(entity);
                            dashComp.IsRootMotion = animationComp.RootMotion;
                        }
                        break;
                    case AnimationTypes.CombatSlot1:
                        if (!_castPool.Value.Has(entity))
                        {
                            ref var attackAnimationComp = ref _attackAnimationPool.Value.Add(entity);
                            attackAnimationComp.AttackAnimationType = AttackAnimationType.combatSlot1;
                            attackAnimationComp.IsRootMotion = animationComp.RootMotion;
                        }
                        break;
                    case AnimationTypes.CombatSlot2:
                        if (!_castPool.Value.Has(entity))
                        {
                            ref var attackAnimationComp = ref _attackAnimationPool.Value.Add(entity);
                            attackAnimationComp.AttackAnimationType = AttackAnimationType.combatSlot2;
                            attackAnimationComp.IsRootMotion = animationComp.RootMotion;
                        }
                        break;
                    case AnimationTypes.CombatSlot3:
                        if (!_castPool.Value.Has(entity))
                        {
                            ref var attackAnimationComp = ref _attackAnimationPool.Value.Add(entity);
                            attackAnimationComp.AttackAnimationType = AttackAnimationType.combatSlot3;
                            attackAnimationComp.IsRootMotion = animationComp.RootMotion;
                        }
                        break;
                    case AnimationTypes.Charge:
                        if (!_castPool.Value.Has(entity))
                        {
                            ref var castComp2 = ref _castPool.Value.Add(entity);
                            castComp2.Type = CastAnimationType.prepare;
                            castComp2.IsRootMotion = animationComp.RootMotion;
                        }
                        break;
                    case AnimationTypes.Recovery:
                        if (!_recoveryAnimationPool.Value.Has(entity))
                        {
                            _recoveryAnimationPool.Value.Add(entity).IsRootMotion = animationComp.RootMotion;
                        }
                        break;
                    case AnimationTypes.Tired:
                        if (!_toughnessAnimationPool.Value.Has(entity))
                        {
                            _toughnessAnimationPool.Value.Add(entity);
                        }
                        break;
                    case AnimationTypes.RunRight:
                        break;
                    case AnimationTypes.RunLeft:
                        break;
                    case AnimationTypes.RunBack:
                        break;
                    case AnimationTypes.Spawn:
                        if (!_spawnPool.Value.Has(entity)) _spawnPool.Value.Add(entity);
                        break;
                }
            }
        }
    }
}

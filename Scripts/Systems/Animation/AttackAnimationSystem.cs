using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client 
{
    sealed class AttackAnimationSystem : MainEcsSystem 
    {
        readonly EcsFilterInject<Inc<AttackAnimationState, AnimatorComponent>, Exc<DashAnimationState, SpawnAnimationState>> _filter = default;
        readonly EcsPoolInject<AttackAnimationState> _animationAttackPool = default;
        readonly EcsPoolInject<AnimatorComponent> _animatorPool = default;

        public override MainEcsSystem Clone()
        {
            return new AttackAnimationSystem();
        }

        public override void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter.Value)
            {
                ref var animatorComp = ref _animatorPool.Value.Get(entity);
                ref var attackAnimationComp = ref _animationAttackPool.Value.Get(entity);

                animatorComp.Animator.applyRootMotion = attackAnimationComp.IsRootMotion;
                animatorComp.Animator.SetBool(AnimatorComponent.IsPrepare, false);

                switch (attackAnimationComp.AttackAnimationType)
                {
                    case AttackAnimationType.attack1:
                        animatorComp.Animator.SetTrigger(AnimatorComponent.Attack1);
                        break;
                    case AttackAnimationType.attack2:
                        animatorComp.Animator.SetTrigger(AnimatorComponent.Attack2);
                        break;
                    case AttackAnimationType.attack3:
                        animatorComp.Animator.SetTrigger(AnimatorComponent.Attack3);
                        break;
                    case AttackAnimationType.specAttack:
                        animatorComp.Animator.SetTrigger(AnimatorComponent.SpecAttack1);
                        break;
                    case AttackAnimationType.specAttack2:
                        animatorComp.Animator.SetTrigger(AnimatorComponent.SpecAttack2);
                        break;
                    case AttackAnimationType.specAttack3:
                        animatorComp.Animator.SetTrigger(AnimatorComponent.SpecAttack3);
                        break;
                    case AttackAnimationType.combatSlot1:
                        animatorComp.Animator.SetTrigger(AnimatorComponent.CombatSlot1);
                        break;
                    case AttackAnimationType.combatSlot2:
                        animatorComp.Animator.SetTrigger(AnimatorComponent.CombatSlot2);
                        break;
                    case AttackAnimationType.combatSlot3:
                        animatorComp.Animator.SetTrigger(AnimatorComponent.CombatSlot3);
                        break;
                }

            }
        }
    }
}
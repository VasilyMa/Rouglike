using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using System.Collections.Generic;
using UnityEngine;

namespace Client {
    sealed class ChangeAnimatorUnitSystem : MainEcsSystem
    {
        readonly EcsFilterInject<Inc<AnimationStateComponent,AbilityUnitComponent>> _filter = default;
        readonly EcsPoolInject<AnimationStateComponent> _changeAnimationPool = default;
        readonly EcsPoolInject<AbilityUnitComponent> _abilityUnitPool = default;
        public override MainEcsSystem Clone()
        {
            return new ChangeAnimatorUnitSystem();
        }
        public override void Run (IEcsSystems systems) {
            foreach (var entity in _filter.Value)
            {
                ref var animationComp = ref _changeAnimationPool.Value.Get(entity);
                ref var abilityUnitComp = ref _abilityUnitPool.Value.Get(entity);

                if (!animationComp.IsUniqueAnimation) return;

                string animationOriginName = string.Empty;

                if (!abilityUnitComp.AbilityUnitMB.TemporaryAnimatorOverrideController)
                {
                    abilityUnitComp.AbilityUnitMB.TemporaryAnimatorOverrideController = Object.Instantiate(abilityUnitComp.AbilityUnitMB.WeaponConfig.AnimatorOverrideController);
                    abilityUnitComp.AbilityUnitMB.TemporaryAnimatorOverrideController.name = "TemporaryAnimatorOverrideController";
                    abilityUnitComp.AbilityUnitMB.Animator.runtimeAnimatorController = abilityUnitComp.AbilityUnitMB.TemporaryAnimatorOverrideController;
                }

                switch (animationComp.AnimationType)
                {
                    case AnimationTypes.Attack:
                            animationOriginName = "common_attack_1";
                        break;
                    case AnimationTypes.Attack2:
                            animationOriginName = "common_attack_2";
                        break;
                    case AnimationTypes.Attack3:
                            animationOriginName = "common_attack_3";
                        break;
                    case AnimationTypes.SpecAttack:
                        animationOriginName = "common_special";
                        break;
                    case AnimationTypes.SpecAttack2:
                        animationOriginName = "common_special_2";
                        break;
                    case AnimationTypes.SpecAttack3:
                        animationOriginName = "common_special_3";
                        break;
                    case AnimationTypes.Dash:
                        animationOriginName = "common_dash_forward";
                        break;
                    case AnimationTypes.CombatSlot1:
                        animationOriginName = "common_combat_slot_1";
                        break;
                    case AnimationTypes.CombatSlot2:
                        animationOriginName = "common_combat_slot_2";
                        break;
                    case AnimationTypes.CombatSlot3:
                        animationOriginName = "common_combat_slot_3";
                        break;
                    case AnimationTypes.Charge:
                        animationOriginName = "";
                        break;
                    case AnimationTypes.Recovery:
                        animationOriginName = "";
                        break;
                    case AnimationTypes.Tired:
                        animationOriginName = "";
                        break;
                    case AnimationTypes.RunRight:
                        break;
                    case AnimationTypes.RunLeft:
                        break;
                    case AnimationTypes.RunBack:
                        break;
                    case AnimationTypes.Spawn:
                        animationOriginName = "";
                        break;
                }

                if (abilityUnitComp.AbilityUnitMB.ClipOverrides == null)
                {  
                    abilityUnitComp.AbilityUnitMB.ClipOverrides = new AnimationClipOverrides(abilityUnitComp.AbilityUnitMB.WeaponConfig.AnimatorOverrideController.overridesCount);
                }

                abilityUnitComp.AbilityUnitMB.TemporaryAnimatorOverrideController.GetOverrides(abilityUnitComp.AbilityUnitMB.ClipOverrides);
                if (!abilityUnitComp.AbilityUnitMB.ClipOverrides[animationOriginName]) return;
                if (abilityUnitComp.AbilityUnitMB.ClipOverrides[animationOriginName].name == animationComp.UniqueAnimation.name)
                {
                    return;
                }
                abilityUnitComp.AbilityUnitMB.ClipOverrides[animationOriginName] = animationComp.UniqueAnimation;
                abilityUnitComp.AbilityUnitMB.TemporaryAnimatorOverrideController.ApplyOverrides(abilityUnitComp.AbilityUnitMB.ClipOverrides);

            }
        }
    }
}
public class AnimationClipOverrides : List<KeyValuePair<AnimationClip, AnimationClip>>
{
    public AnimationClipOverrides(int capacity) : base(capacity) { }

    public AnimationClip this[string name]
    {
        get { return this.Find(x => x.Key.name.Equals(name)).Value; }
        set
        {
            int index = this.FindIndex(x => x.Key.name.Equals(name));
            if (index != -1)
                this[index] = new KeyValuePair<AnimationClip, AnimationClip>(this[index].Key, value);
        }
    }
}

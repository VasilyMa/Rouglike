using System.Collections;
using System.Collections.Generic;

namespace Client
{
    public static class ChangeAnimationController
    {
        private static float TRANSITION_SPEED = 0.05f;
        private static float DURATION = 0.1f;
        private static float Offset = 0;

        public static void ChangeAnimationFunc(AnimationTypes type, int entity, float animationSpeed = 1, bool rootMotion = false, float fixedAnimationTransition = 0.15f, bool nonCheckType = false)
        {
            return;
        }
        public static void HitAnimation(float angle,int entity)
        {
            //todo SANIA ANIMATION
            
        }
    }

    public enum AnimationTypes{
        None, Idle, Move, Attack, Attack2, Attack3, SpecAttack, CombatSlot2, CombatSlot1, Interactions, Dash, GetHit, GetHitRight, GetHitLeft, GetHitBack, KnockBack, GetUp, Aim, Charge, Recovery, Tired, RunRight, RunLeft,RunBack,Terrorize,Spawn, SpecAttack2, SpecAttack3, CombatSlot3
    }
}

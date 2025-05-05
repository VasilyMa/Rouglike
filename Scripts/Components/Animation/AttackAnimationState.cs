namespace Client {
    struct AttackAnimationState 
    {
        public AttackAnimationType AttackAnimationType;
        public bool IsRootMotion;
    }

    public enum AttackAnimationType { attack1, attack2, attack3, specAttack, specAttack2, specAttack3, combatSlot1, combatSlot2, combatSlot3 }
}
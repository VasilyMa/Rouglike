namespace Client {
    struct KnockbackAnimationState 
    {
        public KnockbackState KnockbackState;
        public bool IsRootMotion;
    }

    public enum KnockbackState { knockback, getup}
}
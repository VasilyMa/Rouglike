namespace Client 
{
    struct CastAnimationState 
    {
        public CastAnimationType Type;
        public bool IsRootMotion;
    }

    public enum CastAnimationType { prepare, cast }
}
namespace Client {
    struct HitAnimationState 
    {
        public HitAnimationType Type;
        public bool IsRootMotion;
    }
}

public enum HitAnimationType { GetHitFront, GetHitRight, GetHitLeft, GetHitBack }
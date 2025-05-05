
using UnityEngine;

namespace Client {
    struct AnimationStateComponent
    {
        public AnimationTypes AnimationType;
        public bool RootMotion;
        public bool IsUniqueAnimation;
        public AnimationClip UniqueAnimation;
        public AnimationClipOverrides clipOverrides;
        public string PreviousUniqueAnimationName;
        public AnimatorOverrideController OriginalOverrideController;
    }
}
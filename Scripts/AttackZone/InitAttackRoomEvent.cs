using AbilitySystem;
using System.Collections.Generic;

namespace Client {
    struct InitAttackRoomEvent {
        public float TimeToResolve;
        public float LifeTime;
        public float Delay;
        public List<IAbilityEffect> Components;
    }
}
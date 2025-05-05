using UnityEngine;

namespace Client {
    struct ReactionEvent {
        public int EntitySender;
        public int EntityOwner;
        public float PushForce;
        public bool IsSpecAttack;
        public bool IsAnimationInvoke;
    }
}
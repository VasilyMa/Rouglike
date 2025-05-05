using Leopotam.EcsLite;
using UnityEngine;

namespace Client {
    struct DamageOverTimeEffect {
        public int DamageOverTime;
        public float Duration;
        public float timer;
        public float durationTimer;
        public GameObject Paricle;
        public EcsPackedEntity SenderEntity;
    }
}
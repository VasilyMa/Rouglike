using Leopotam.EcsLite;

namespace Client {
    struct TargetMissileComponent {
        public EcsPackedEntity EntityTarget;
    }
    struct CasterMissileComponent
    {
        public EcsPackedEntity EntityCaster;
    }
}
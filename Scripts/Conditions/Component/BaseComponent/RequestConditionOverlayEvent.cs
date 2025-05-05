using Leopotam.EcsLite;

namespace Client {
    struct RequestConditionOverlayEvent {
        public EcsPackedEntity OwnerEntity;
        public Condition Condition;
        public int StartCountPoint;
    }
}
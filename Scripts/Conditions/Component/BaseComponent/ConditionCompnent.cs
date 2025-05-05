using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client {
    struct ConditionCompnent {
        public EcsPackedEntity PackedEntityOwner;
        public Condition Condition;
    }
}
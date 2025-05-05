using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client {
    struct AddHitIntervalEvent {
        public HitAnimationType Type;
        public EcsPackedEntity TargetEntity;
    }
    struct CalculationHitIntervalEvent
    {
        public HitAnimationType Type;
    }
}
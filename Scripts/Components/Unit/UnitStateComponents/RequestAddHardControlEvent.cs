using Leopotam.EcsLite;
namespace Client {
    struct RequestAddHardControlEvent {
        public EcsPackedEntity TargetEntity;
        public float ControlTime;
        public bool TheTimerShouldBeSetToTheTimeUntilTheEndOfTheAnimation;
    }
}
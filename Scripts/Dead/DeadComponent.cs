using System.Threading;
using Leopotam.EcsLite;

namespace Client {
    struct DeadComponent
    {
        public float TimerToDestroy;
        public float TimeOfDeath;
        public EcsPackedEntity KillerEntity;
    }
}
using AbilitySystem;
using Leopotam.EcsLite;
using System.Collections.Generic;

namespace Client {
    struct TargetsContext {
        public float closestEnemyDistance;
        public EcsPackedEntity closestEnemyEntity;
    }
}
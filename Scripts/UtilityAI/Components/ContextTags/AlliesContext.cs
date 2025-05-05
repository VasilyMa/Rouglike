using Leopotam.EcsLite;
using System.Collections.Generic;
using UnityEngine;

namespace Client {
    struct AlliesContext {
        public Dictionary<EcsPackedEntity, float> alliedEntitiesWithDistance;
    }
}
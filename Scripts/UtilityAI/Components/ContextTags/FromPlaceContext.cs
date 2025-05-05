using Leopotam.EcsLite;
using System.Collections.Generic;
using UnityEngine;
namespace Client {
    struct FromPlaceContext {
        public List<EcsPackedEntity> fromPlaceAbilitiesList;
        public List<EcsPackedEntity> validAbilitiesList;
        public bool AnyActionUsable;
        public List<Transform> fromPlaceTransforms;
    }
}
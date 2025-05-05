using Leopotam.EcsLite;
using System.Collections.Generic;

namespace Client {
    struct TerrorizeContext {
        public List<EcsPackedEntity> terrorizeAbilitiesList;
        public List<EcsPackedEntity> validAbilitiesList;
        public bool anyActionUsable;
    }
}
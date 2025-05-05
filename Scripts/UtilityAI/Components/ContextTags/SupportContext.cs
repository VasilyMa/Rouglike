using Leopotam.EcsLite;
using System.Collections.Generic;

namespace Client {
    struct SupportContext {
        public bool anyActionAvailable;
        public bool anyActionUsable;
        public List<EcsPackedEntity> supportAbilitiesList;
    }
}
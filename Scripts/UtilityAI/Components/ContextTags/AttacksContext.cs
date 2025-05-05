using Leopotam.EcsLite;
using System.Collections.Generic;

namespace Client {
    struct AttacksContext {
        public List<EcsPackedEntity> attackAbilitiesList;
        public List<EcsPackedEntity> validAbilitiesList;
        public bool anyActionAvailable;
        public bool anyActionUsable;
        
    }
}
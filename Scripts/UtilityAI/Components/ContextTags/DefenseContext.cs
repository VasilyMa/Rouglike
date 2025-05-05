using Leopotam.EcsLite;
using System.Collections.Generic;

namespace Client {
    struct DefenseContext {
        public List<EcsPackedEntity> defenseActionsList;
        public bool anyActionAvailable;
    }
}
using System.Collections.Generic;
using Leopotam.EcsLite;
namespace Client {
    struct HelperPerk {
        public List<IPerkResolveEffect> ResolveEffects;
        public EcsPackedEntity OwnerPerkEntity;
    }
}
using System.Collections.Generic;
namespace Client {
    struct PerkResolveBlockComponent {
        public List<IPerkResolveEffect> ResolveEffectsOriginal;
        public List<IPerkResolveEffect> ResolveEffectsModified;
        public List<IDisposeResolvePerkCondition> DisposeEffectsOriginal;
        public List<IDisposeResolvePerkCondition> DisposeEffectsModified;
    }
}
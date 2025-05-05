using System.Collections.Generic;
using Leopotam.EcsLite;
using AbilitySystem;
namespace Client {
    struct DoResolveBlockEvent {
        public List<IAbilityEffect> Components;
        public EcsPackedEntity SenderPackedEntity;

    }
}
using Leopotam.EcsLite;
using System.Collections.Generic;

namespace Client {
    struct ConditionContainerComponent {
        public Dictionary<Condition, EcsPackedEntity> Conditions;
    }
}
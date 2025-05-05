using System.Collections.Generic;
using Leopotam.EcsLite;
namespace Client {
    public struct UnitCollisionEvent {
        public List<EcsPackedEntity> CollisionEntity;
        public EcsPackedEntity SenderPackedEntity;
    }
}
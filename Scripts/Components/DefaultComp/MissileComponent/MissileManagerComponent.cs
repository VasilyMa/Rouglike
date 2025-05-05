using AbilitySystem;
using Leopotam.EcsLite;
using System.Collections.Generic;
using UnityEngine;

namespace Client {
    struct MissileManagerComponent 
    {
        public EcsPackedEntity OwnerPackedEntity;
        public EcsPackedEntity AbilityPackedEntity;
        public List<IAbilityMissileComponent> Components;
        public int currentInedexComponent;
        public float charge;
    }
}
using System.Collections.Generic;
using AbilitySystem;
using Leopotam.EcsLite;
using UnityEngine;

namespace Client 
{
    struct AttackZoneComponent
    {
        public Mesh AttackMesh;
        [HideInInspector] public Mesh AttackZoneMesh;
        [HideInInspector] public AttackZone AttackZone;
        [HideInInspector] public EcsPackedEntity OwnerEntity;
        [HideInInspector] public EcsPackedEntity AbilityEntity;
    }
}
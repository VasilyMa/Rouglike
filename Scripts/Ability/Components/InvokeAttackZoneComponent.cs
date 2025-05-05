using System.Collections.Generic;
using AbilitySystem;
using Leopotam.EcsLite;
using UnityEngine;

namespace Client 
{
    struct InvokeAttackZoneEvent
    {
        [HideInInspector] public Mesh AttackZoneMesh;
        [HideInInspector] public EcsPackedEntity OwnerEntity;
        [HideInInspector] public EcsPackedEntity AbilityEntity;
        [HideInInspector] public Vector3 Size;
        [HideInInspector] public float DisableTime;

        public void Init(EcsPackedEntity abilityPackedEntity, EcsPackedEntity targetPackedEntity, Mesh mesh, Vector3 size, float disableTime)
        {
            AbilityEntity = abilityPackedEntity;
            OwnerEntity = targetPackedEntity;
            AttackZoneMesh = mesh;
            Size = size;
            if(disableTime == 0f)
            {
                disableTime = 0.2f;
            }
            DisableTime = disableTime;

        }
    }
}
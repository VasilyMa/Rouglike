using Leopotam.EcsLite;
using AbilitySystem;
using System.Collections.Generic;
using UnityEngine;

namespace Client {
    struct InvokeMissileEvent {
        [HideInInspector] public EcsPackedEntity OwnerPackedEntity;
        [HideInInspector] public EcsPackedEntity AbilityPackedEntity;
        [SerializeReference] public List<IAbilityMissileComponent> Components;
        public MissileMB missile;
        public float Speed;
        public Vector3 Offset;
        [HideInInspector] public string LayerNameTarget;
        public void Init(EcsPackedEntity abilityPackedEntity,EcsPackedEntity ownerPackedEntity, List<IAbilityMissileComponent> list)
        {
            AbilityPackedEntity = abilityPackedEntity;
            OwnerPackedEntity = ownerPackedEntity;
            Components = new List<IAbilityMissileComponent>(list);
        }

    }
}
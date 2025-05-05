using AbilitySystem;
using System.Collections.Generic;
using UnityEngine;
using Leopotam.EcsLite;

namespace Client {
    struct InvokeVisualEffectEvent 
    {
        public bool IsParentTransformInvoke;
        public Vector3 offset;
        public Vector3 RotationOffset;
        public Transform Parent;
        public SourceParticle Particle;
        public Vector3 StartPos;
        public Quaternion StartRot;
        public EcsPackedEntity EntityCaster;
        public EcsPackedEntity AbilityEntity;
    }
}
using AbilitySystem;
using Leopotam.EcsLite;
using System.Collections.Generic;
using UnityEngine;
using Statement;

namespace Client {
    struct RequestVFXEvent : IAbilityComponent
    {
        [SerializeField] public bool IsParentTransformInvoke;
        [SerializeField] public Vector3 offset;
        [SerializeField] public Vector3 RotationOffset;
        [SerializeField] public List<SourceParticle> sourceParticle;
        [HideInInspector] public EcsPackedEntity TargetPackedEntity;
        [HideInInspector] public EcsPackedEntity AbilityEntity;

        public void Dispose(int entityCaster,int abilityEntity, EcsWorld world)
        {
            ref var visualComp = ref State.Instance.EcsRunHandler.World.GetPool<VisualEffectsComponent>().Get(abilityEntity);
            foreach (var _sourceParticle in visualComp.SourceParticles)
            {
                _sourceParticle.Dispose();
            }
            visualComp.SourceParticles.Clear();
        }

        public void Init()
        {
        }

        public void Invoke(int ownerEntity, int abilityEntity, EcsWorld world, float charge = 1)
        {
            ref var requestComp = ref world.GetPool<RequestVFXEvent>().Add(world.NewEntity());
            requestComp.TargetPackedEntity = world.PackEntity(ownerEntity);
            requestComp.IsParentTransformInvoke = IsParentTransformInvoke;
            requestComp.offset = offset;
            requestComp.sourceParticle = sourceParticle;
            requestComp.RotationOffset = RotationOffset;
            requestComp.AbilityEntity = world.PackEntity(abilityEntity);
        }
    }
}